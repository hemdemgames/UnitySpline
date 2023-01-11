using System;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    public abstract class Spline
    {
        [SerializeField] private SplineBuild splineBuild;

        [field: SerializeField] public bool loop { get; private set; }

        public event Action OnRebuild;

        public void SetLoop(bool loop)
        {
            this.loop = loop;
            Rebuild();
        }
        
        public abstract ISplineGizmo Gizmo { get; }
        #if UNITY_EDITOR
        public abstract ISplineEditor Editor { get; }
        #endif

        private SplineBuild SplineBuild
        {
            get
            {
                splineBuild ??= Build();
                return splineBuild;
            }
        }

        public SplineSample EvaluateByDistance(float distance)
        {
            return SplineBuild.EvaluateByDistance(distance);
        }

        public SplineSample EvaluateByPercent(float percent)
        {
            float distance = SplineBuild.Length * percent;
            return EvaluateByDistance(distance);
        }

        public void Project(Vector3 point, float precision, out float percent)
        {
            splineBuild.Project(point, precision, out percent);
        }

        public void GetClosestPoint(Vector3 point, float precision, out SplineSample sample)
        {
            splineBuild.GetClosestPoint(point, precision, out sample);
        }

        public float Length => SplineBuild.Length;
        public SplineFragment[] Fragments => splineBuild.Fragments;

        public void Rebuild()
        {
            splineBuild = Build();
            OnRebuild?.Invoke();
        }

        protected abstract SplineBuild Build();
    }
}