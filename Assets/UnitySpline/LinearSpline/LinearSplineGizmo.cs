using HemdemGames.SplineTool.Behaviours;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    public class LinearSplineGizmo : ISplineGizmo
    {
        private LinearSpline spline;

        public LinearSplineGizmo(LinearSpline spline)
        {
            this.spline = spline;
        }

        public Color Color { get; set; } = SplineSettings.LineColor;
        public bool DisplayNormals { get; set; } = true;
        public bool DisplayArrows { get; set; } = true;

        public void Draw()
        {
            var fragments = spline.Fragments;
            for (int i = 0; i < fragments.Length; i++)
            {
                Quaternion rot = fragments[i].EvaluateRotation(0);
                Vector3 start = fragments[i].EvaluatePosition(0);
                Vector3 end = fragments[i].EvaluatePosition(1);
                Gizmos.DrawLine(start, end);
                
                if (DisplayNormals)
                {
                    Vector3 normal = rot * Vector3.up;
                    Gizmos.color = SplineSettings.NormalColor;
                    Gizmos.DrawLine(start, start + normal);
                }

                if (DisplayArrows)
                {
                    Gizmos.color = SplineSettings.ArrowColor;
                    SplineGizmoUtility.DrawArrow(start, rot);
                }
            }
        }
    }
}