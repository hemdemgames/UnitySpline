using UnityEngine;

namespace HemdemGames.SplineTool
{
    public static class SplineGizmoUtility
    {
        public static void DrawArrow(Vector3 point, Quaternion rotation, float angle = 30, float length = 1)
        {
            Vector3 leftPaddlePoint = point + (rotation * Quaternion.Euler(0, angle, 0)) * (Vector3.back * length);
            Vector3 rightPaddlePoint = point + (rotation * Quaternion.Euler(0, -angle, 0)) * (Vector3.back * length);
            Gizmos.DrawLine(point, leftPaddlePoint);
            Gizmos.DrawLine(point, rightPaddlePoint);
        }
    }
}