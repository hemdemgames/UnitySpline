using UnityEngine;

namespace HemdemGames.SplineTool
{
    public static class SplineConvertUtility
    {
        public static LinearSpline ConvertToLinearSpline(BezierSpline bezierSpline)
        {
            LinearSpline linearSpline = new LinearSpline();

            for (int i = 0; i < bezierSpline.Points.Count; i++)
            {
                var point = new SplinePoint();
                point.position = bezierSpline.Points[i].position;
                point.rotation = bezierSpline.Points[i].rotation;
                point.scale = bezierSpline.Points[i].scale;
                linearSpline.Points.Add(point);
            }

            linearSpline.Rebuild();
            return linearSpline;
        }

        public static BezierSpline ConvertToBezierSpline(LinearSpline linearSpline)
        {
            BezierSpline bezierSpline = new BezierSpline();

            for (int i = 0; i < linearSpline.Points.Count; i++)
            {
                var point = new BezierSplinePoint();
                point.position = linearSpline.Points[i].position;
                point.rotation = linearSpline.Points[i].rotation;
                point.scale = linearSpline.Points[i].scale;
                point.SetLeftTangent(Vector3.left);
                point.SetRightTangent(Vector3.right);
                
                if (i > 0)
                {
                    BezierSplinePoint previousPoint = bezierSpline.Points[i - 1];
                    Vector3 forward = point.position - previousPoint.position;
            
                    point.SetLeftTangent(-forward * .5f);
                    point.SetRightTangent(forward * .5f);
                }
                
                bezierSpline.Points.Add(point);
            }

            bezierSpline.Rebuild();
            return bezierSpline;
        }
    }
}