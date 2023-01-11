
using UnityEngine;

namespace HemdemGames.SplineTool
{
    public class LinearSplineBuilder : ISplineBuilder
    {
        private LinearSpline spline;

        public LinearSplineBuilder(LinearSpline spline)
        {
            this.spline = spline;
        }

        public SplineBuild Build()
        {
            int splinePointCount = spline.Points.Count;
            int fragmentCount =  Mathf.Max(splinePointCount - 1, 0);
            if (spline.loop) { fragmentCount++; }
            LinearSplineFragment[] fragments = new LinearSplineFragment[fragmentCount];

            for (int i = 0; i < fragments.Length; i++)
            {
                fragments[i] =  new LinearSplineFragment(spline.Points[i], spline.Points[(i + 1) % splinePointCount]);
            }
            
            return new SplineBuild(fragments);
        }
    }
}