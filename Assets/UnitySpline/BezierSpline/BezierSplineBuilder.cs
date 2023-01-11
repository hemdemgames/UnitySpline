
using UnityEngine;

namespace HemdemGames.SplineTool
{
    public class BezierSplineBuilder : ISplineBuilder
    {
        private BezierSpline spline;

        public BezierSplineBuilder(BezierSpline spline)
        {
            this.spline = spline;
        }

        public SplineBuild Build()
        {
            int splinePointCount = spline.Points.Count;
            int fragmentCount =  Mathf.Max(splinePointCount - 1, 0);
            if (spline.loop) { fragmentCount++; }
            BezierSplineFragment[] fragments = new BezierSplineFragment[fragmentCount];

            for (int i = 0; i < fragments.Length; i++)
            {
                fragments[i] =  new BezierSplineFragment(spline.Points[i], spline.Points[(i + 1) % splinePointCount]);
            }
            
            return new SplineBuild(fragments);
        }
    }
}