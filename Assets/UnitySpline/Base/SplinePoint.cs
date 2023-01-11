using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  HemdemGames.SplineTool
{
    [Serializable]
    public class SplinePoint
    {
        public Vector3 position = Vector3.zero;
        public Vector3 scale = Vector3.one;
        public Quaternion rotation = Quaternion.identity;
        public float angle = 0;
        
        public SplinePoint() : this(Vector3.zero) { }
        
        public SplinePoint(Vector3 position) : this(position, Quaternion.identity) { }
        
        public SplinePoint(Vector3 position, Quaternion rotation) : this(position, rotation, Vector3.one) { }

        public SplinePoint(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public Vector3 InverseTransformPoint(Vector3 worldPos)
        {
            return Quaternion.Inverse(rotation) * (worldPos - position);
        }

        public Vector3 TransformPoint(Vector3 localPoint)
        {
            return position + (rotation * localPoint);
        }
    }
}
