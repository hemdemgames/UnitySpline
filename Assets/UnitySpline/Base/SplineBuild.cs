using System;
using System.Linq;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    [Serializable]
    public class SplineBuild
    {
        [SerializeField] private float length;
        [SerializeReference] private SplineFragment[] fragments;

        public float Length => length;
        public SplineFragment[] Fragments => fragments;
        
        public SplineBuild(SplineFragment[] fragments)
        {
            this.fragments = fragments;
            this.length = fragments.Sum(fragment => fragment.GetLength());
        }

        public void GetClosestPoint(Vector3 point, float precision, out SplineSample sample)
        {
            Project(point, precision, out float percent);
            sample = EvaluateByPercent(percent);
        }

        public void Project(Vector3 point, float precision, out float percent)
        {
            int iteration = 4;
            int slice = Mathf.FloorToInt(Length / precision / Mathf.Pow(iteration, iteration));

            float startPercent = 0;
            float endPercent = 1;
            
            for (int i = 0; i < iteration; i++)
            {
                float newStartPercent = startPercent;
                float newEndPercent = endPercent;
                float distance = Mathf.Infinity;
                
                for (int j = 0; j <= slice; j++)
                {
                    float tick = Mathf.Lerp(startPercent, endPercent, (float)j / slice);
                    var sample = EvaluateByPercent(tick);

                    float dist = Vector3.Distance(sample.Position, point);
                    if (dist < distance)
                    {
                        distance = dist;

                        float newStartTick = (float)(j - 1) / slice;
                        float newEndTick = (float)(j + 1) / slice;
                        
                        newStartPercent = Mathf.Lerp(startPercent, endPercent, newStartTick);
                        newStartPercent = Mathf.Max(newStartPercent, startPercent);
                        
                        newEndPercent = Mathf.Lerp(startPercent, endPercent, newEndTick);
                        newEndPercent = Mathf.Min(newEndPercent, endPercent);
                    }
                }

                startPercent = newStartPercent;
                endPercent = newEndPercent;
            }

            percent = Mathf.Lerp(startPercent, endPercent, .5f);
        }

        public SplineSample EvaluateByPercent(float percent)
        {
            return EvaluateByDistance(Length * percent);
        }
        
        public SplineSample EvaluateByDistance(float distance)
        {
            DistanceToFragment(distance, out var fragment, out float t);

            SplineSample fragmentSample = fragment.Evaluate(t);
            float percent = distance / Length;
            
            return new SplineSample(fragmentSample.Position, fragmentSample.Rotation, fragmentSample.Scale, percent, distance);
        }

        private bool DistanceToFragment(float distance, out SplineFragment fragment, out float t)
        {
            distance = Mathf.Clamp(distance, 0, Length);

            fragment = null;
            
            for (int i = 0; i < fragments.Length; i++)
            {
                fragment = fragments[i];
                
                if (i == fragments.Length - 1) break;
                if (distance <= fragment.GetLength()) break;

                distance -= fragment.GetLength();
            }

            distance = Mathf.Clamp(distance, 0, fragment.GetLength());

            t = distance / fragment.GetLength();
            return true;
        }
    }
}