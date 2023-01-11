using System;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    [Serializable]
    public class BezierSplineFragment : SplineFragment
    {
        [SerializeField] private BezierSplinePoint start;
        [SerializeField] private BezierSplinePoint end;
        
        public BezierSplineFragment(BezierSplinePoint start, BezierSplinePoint end)
        {
            this.start = start;
            this.end = end;
        }

        public override Vector3 EvaluatePosition(float t)
        {
            Vector3 p0 = start.position;
            Vector3 p1 = start.TransformPoint(start.rightTangent);
            Vector3 p2 = end.TransformPoint(end.leftTangent);
            Vector3 p3 = end.position;
            
            Vector3 a = Vector3.Lerp(p0, p1, t);
            Vector3 b = Vector3.Lerp(p1, p2, t);
            Vector3 c = Vector3.Lerp(p2, p3, t);

            Vector3 d = Vector3.Lerp(a, b, t);
            Vector3 e = Vector3.Lerp(b, c, t);
            
            return Vector3.Lerp(d, e, t);
        }

        public override Quaternion EvaluateRotation(float t)
        {
            Vector3 currentPos = EvaluatePosition(Mathf.Min(t, 0.99f));
            Vector3 nextPos = EvaluatePosition(Mathf.Min(t + .01f, 1));

            Vector3 forward = (nextPos - currentPos).normalized;

            float angle = Mathf.Lerp(start.angle, end.angle, t);
            return Quaternion.LookRotation(forward) * Quaternion.Euler(0,0, angle);
        }

        public override Vector3 EvaluateScale(float t)
        {
            return Vector3.Lerp(start.scale, end.scale, t);
        }
    }
}