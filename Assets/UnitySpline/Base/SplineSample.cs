using System;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    [Serializable]
    public struct SplineSample
    {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;
        private float percent;
        private float distance;

        public Vector3 Position => position;
        public Quaternion Rotation => rotation;
        public Vector3 Scale => scale;
        public float Percent => percent;
        public float Distance => distance;

        public SplineSample(Vector3 pos, Quaternion rot, Vector3 scale, float percent, float distance)
        {
            this.position = pos;
            this.rotation = rot;
            this.scale = scale;
            this.percent = percent;
            this.distance = distance;
        }
    }
}