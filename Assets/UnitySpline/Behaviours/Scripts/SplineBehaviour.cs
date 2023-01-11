using System;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public enum PlacementMode
    {
        Surface,
        CameraView
    }
    
    public class SplineBehaviour : MonoBehaviour
    {
        [SerializeReference, HideInInspector]
        protected Spline spline = new BezierSpline();
        
        public float Length => spline.Length;
        public bool IsLoop => spline.loop;

        public event Action OnRebuild
        {
            add => spline.OnRebuild += value;
            remove => spline.OnRebuild -= value;
        }

        public void GetClosestPoint(Vector3 worldPos, float precision, out SplineSample sample)
        {
            Vector3 localPos = transform.InverseTransformPoint(worldPos);
            spline.GetClosestPoint(localPos, precision, out sample);
        }

        public void Project(Vector3 worldPos, float precision, out float percent)
        {
            Vector3 localPos = transform.InverseTransformPoint(worldPos);
            spline.Project(localPos, precision, out percent);
        }
        
        public SplineSample Evaluate(float percent)
        {
            var sample = spline.EvaluateByPercent(percent);
            var pos = transform.TransformPoint(sample.Position);
            var rot = transform.rotation * sample.Rotation;
            return new SplineSample(pos, rot, sample.Scale, sample.Percent, sample.Distance);
        }

        public SplineSample EvaluateByDistance(float distance)
        {
            var sample = spline.EvaluateByDistance(distance);
            var pos = transform.TransformPoint(sample.Position);
            var rot = transform.rotation * sample.Rotation;
            return new SplineSample(pos, rot, sample.Scale, sample.Percent, sample.Distance);
        }
        
        public void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            spline.Gizmo.Draw();
        }
    }
}