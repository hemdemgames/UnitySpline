using System;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    public interface ISplineEditor
    {
        int PointCount { get; }
        void AddPoint(Vector3 position);
        void RemovePoint(int index);
        void SetPointPosition(int index, Vector3 position);
        Vector3 GetPointPosition(int index);
        void SetPointAngle(int index, float angle);
        void Reverse();
        float GetPointAngle(int index);
        int GetPointControlID(int index, FocusType type);
        void DrawPointCap(int index, out int controlId);
        void DrawPointMoveHandle(int pointIndex, Action onBeforeChange, Action onChanged);
    }
}