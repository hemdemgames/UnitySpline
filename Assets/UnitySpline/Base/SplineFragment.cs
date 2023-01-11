using System;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    [Serializable]
    public abstract class SplineFragment : ISplineFragment
    {
        [SerializeField] private float length;
        [SerializeField] private bool isLengthCalculated;
        
        private AnimationCurve uniformTimeCurve;

        protected float UniformT(float t)
        {
            uniformTimeCurve ??= GenerateUniformTimeCurve();
            return uniformTimeCurve.Evaluate(GetLength() * t);
        } 
        
        public float GetLength()
        {
            if (!isLengthCalculated) CalculateLength();
            isLengthCalculated = true;
            return length;
        }

        public void RecalculateLength()
        {
            isLengthCalculated = true;
            CalculateLength();
        }

        private AnimationCurve GenerateUniformTimeCurve()
        {
            var curve = new AnimationCurve();
            curve.AddKey(0, 0);

            int samples = 12;
            float distance = 0;

            for (int i = 1; i < samples; i++)
            {
                Vector3 a = EvaluatePosition((float)(i - 1) / samples);
                Vector3 b = EvaluatePosition((float)i / samples);

                distance += Vector3.Distance(a, b);
                float time = (float)i / samples;

                curve.AddKey(distance, time);
            }

            curve.AddKey(GetLength(), 1);
            return curve;
        }
        
        private void CalculateLength(int samples = 12)
        {
            float range = 1f / samples;

            for (int i = 0; i < samples; i++)
            {
                Vector3 currentPoint = EvaluatePosition(range * i);
                Vector3 nextPoint = EvaluatePosition(range * (i + 1));
                length += Vector3.Distance(currentPoint, nextPoint);
            }
        }
        
        public abstract Vector3 EvaluatePosition(float t);
        public abstract Quaternion EvaluateRotation(float t);
        public abstract Vector3 EvaluateScale(float t);

        public Vector3 EvaluatePositionUniform(float t)
        {
            return EvaluatePosition(UniformT(t));
        }
        
        public SplineSample Evaluate(float t)
        {
            float distance = GetLength() * t;
            float uniformT = UniformT(t);
            Vector3 position = EvaluatePosition(uniformT);
            Quaternion rotation = EvaluateRotation(uniformT);
            Vector3 scale = EvaluateScale(uniformT);
            return new SplineSample(position, rotation, scale, t, distance);
        }
    }
}