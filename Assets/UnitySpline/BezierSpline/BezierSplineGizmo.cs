using HemdemGames.SplineTool.Behaviours;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    public class BezierSplineGizmo : ISplineGizmo
    {
        private int samples = 12;
        private BezierSpline spline;
        
        public BezierSplineGizmo(BezierSpline spline)
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
                for (int j = 0; j < samples; j++)
                {
                    Quaternion rot = fragments[i].EvaluateRotation((float)j / samples);
                    Vector3 start = fragments[i].EvaluatePosition((float)j / samples);
                    
                    Vector3 end = fragments[i].EvaluatePosition((float)(j + 1) / samples);
                    Gizmos.color = Color;
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
}