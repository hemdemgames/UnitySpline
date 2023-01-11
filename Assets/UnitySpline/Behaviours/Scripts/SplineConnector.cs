using System;
using System.Reflection;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineConnector : SplineBehaviour
    {
        [SerializeField] private SplineBehaviour mainSpline;
        [SerializeField] private SplineBehaviour targetSpline;
        [SerializeField, Range(0f, 1f)] private float targetSplinePercent;

        private BezierSpline bezierSpline => spline as BezierSpline;

        public SplineBehaviour TargetSpline => targetSpline;
        public float TargetSplinePercent => targetSplinePercent;
        
        private void OnEnable()
        {
            mainSpline.OnRebuild += UpdateConnection;
            targetSpline.OnRebuild += UpdateConnection;
            SplineConnectionManagement.AddSplineConnection(mainSpline, this);
        }

        private void OnDisable()
        {
            mainSpline.OnRebuild -= UpdateConnection;
            targetSpline.OnRebuild -= UpdateConnection;
            SplineConnectionManagement.RemoveSplineConnection(mainSpline, this);
        }

        private void OnValidate()
        {
            UpdateConnection();
        }

        public void UpdateConnection()
        {
            if (!mainSpline || !targetSpline) return;

            if (spline is not BezierSpline) spline = new BezierSpline();
            
            for (int i = bezierSpline.Points.Count; i < 2; i++)
                bezierSpline.Points.Add(new BezierSplinePoint());

            bezierSpline.Points[0].position = transform.InverseTransformPoint(mainSpline.Evaluate(1).Position);
            bezierSpline.Points[1].position = transform.InverseTransformPoint(targetSpline.Evaluate(targetSplinePercent).Position);
            bezierSpline.Rebuild();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            spline.Gizmo.Color = Color.magenta;
            spline.Gizmo.DisplayArrows = false;
            spline.Gizmo.DisplayNormals = false;
            spline.Gizmo.Draw();
            
            mainSpline.OnDrawGizmosSelected();
            targetSpline.OnDrawGizmosSelected();
        }
    }
}