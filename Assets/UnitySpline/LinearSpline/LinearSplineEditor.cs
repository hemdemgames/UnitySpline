using System;
using HemdemGames.SplineTool.Behaviours;
using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    public class LinearSplineEditor : ISplineEditor
    {
        private LinearSpline spline;

        public LinearSplineEditor(LinearSpline spline)
        {
            this.spline = spline;
        }

        public int PointCount => spline.Points.Count;
        
        public void AddPoint(Vector3 position)
        {
            spline.Points.Add(new SplinePoint()
            {
                position = position
            });
        }

        public void RemovePoint(int index)
        {
            spline.Points.Remove(index);
        }

        public void SetPointPosition(int index, Vector3 position)
        {
            spline.Points[index].position = position;
        }

        public Vector3 GetPointPosition(int index)
        {
            return spline.Points[index].position;
        }

        public void SetPointAngle(int index, float angle)
        {
            spline.Points[index].angle = angle;
        }

        public void Reverse()
        {
            spline.Points.Reverse();
        }

        public float GetPointAngle(int index)
        {
            return spline.Points[index].angle;
        }

        public int GetPointControlID(int index, FocusType type)
        {
            return GUIUtility.GetControlID(spline.Points[index].GetHashCode(), type);
        }

        public void DrawPointCap(int index, out int controlId)
        {
            SplinePoint point = spline.Points[index];
            controlId = GUIUtility.GetControlID(point.GetHashCode(), FocusType.Passive);
            Handles.SphereHandleCap(0, point.position, Quaternion.identity, SplineSettings.PointRadius, Event.current.type);
        }

        public void DrawPointMoveHandle(int pointIndex, Action onBeforeChange, Action onChanged)
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                SplinePoint point = spline.Points[pointIndex];
                Vector3 position = Handles.PositionHandle(point.position, Quaternion.identity);

                if (!scope.changed) return;
                
                onBeforeChange?.Invoke();
                point.position = position;
                onChanged?.Invoke();
            }
        }
    }
}