using UnityEngine;

namespace HemdemGames.SplineTool
{
    public interface ISplineFragment
    {
        Vector3 EvaluatePosition(float distance);
        Quaternion EvaluateRotation(float distance);
        Vector3 EvaluateScale(float distance);
    }
}