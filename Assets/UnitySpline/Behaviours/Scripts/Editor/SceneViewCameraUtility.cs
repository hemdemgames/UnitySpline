using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public static class SceneViewCameraUtility
    {
        public static bool Raycast(out RaycastHit hit)
        {
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            return Physics.Raycast(ray, out hit);
        }

        public static Vector3 GetMouseWorldPosition(float depth)
        {
            return HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).GetPoint(depth);
        }
    }
}