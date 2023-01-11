using System;
using System.Reflection;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineProjector : MonoBehaviour
    {
        [SerializeField] private bool updateOnlyRuntime = true;
        [SerializeField] private float gizmoRadius = 1f;
        [SerializeField] private SplineBehaviour spline;

        private SplineSample projectResult;

        private void Update()
        {
            spline.GetClosestPoint(transform.position, .1f, out projectResult);
        }

        private void OnDrawGizmosSelected()
        {
            spline.OnDrawGizmosSelected();

            if (updateOnlyRuntime) return;
            
            Update();
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(projectResult.Position, gizmoRadius);
        }
    }
}