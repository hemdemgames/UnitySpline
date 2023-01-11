using System;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    [Serializable]
    public class LinearSplineFragment : SplineFragment
    {
        [SerializeField] private SplinePoint start;
        [SerializeField] private SplinePoint end;
        
        public LinearSplineFragment(SplinePoint start, SplinePoint end)
        {
            this.start = start;
            this.end = end;
        }

        public override Vector3 EvaluatePosition(float t)
        {
            return Vector3.Lerp(start.position, end.position, t);
        }

        public override Quaternion EvaluateRotation(float t)
        {
            float angle = Mathf.Lerp(start.angle, end.angle, t);
            Vector3 forward = end.position - start.position;
            
            return Quaternion.LookRotation(forward) * Quaternion.Euler(0,0, angle);
        }

        public override Vector3 EvaluateScale(float t)
        {
            return Vector3.Lerp(start.scale, end.scale, t);
        }
    }
}