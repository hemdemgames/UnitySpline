using System;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    public enum TangentMode
    {
        Free,
        Mirror
    }
    
    [Serializable]
    public class BezierSplinePoint : SplinePoint
    {
        [field:SerializeField] public Vector3 leftTangent { get; private set; }  = Vector3.left;
        [field:SerializeField] public Vector3 rightTangent { get; private set; } = Vector3.right;
        [field:SerializeField] public TangentMode tangentMode { get; private set; } = TangentMode.Mirror;

        public void SetTangentMode(TangentMode tangentMode)
        {
            if (tangentMode == TangentMode.Mirror)
                leftTangent = rightTangent;
            
            this.tangentMode = tangentMode;
        }

        public void SetLeftTangent(Vector3 tangent)
        {
            leftTangent = tangent;

            if (tangentMode == TangentMode.Mirror)
                rightTangent = -tangent;
        }

        public void SetRightTangent(Vector3 tangent)
        {
            rightTangent = tangent;
            
            if (tangentMode == TangentMode.Mirror)
                leftTangent = -tangent;
        }
    }
}