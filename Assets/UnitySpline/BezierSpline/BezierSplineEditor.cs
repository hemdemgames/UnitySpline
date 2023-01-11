using System;
using System.Collections.Generic;
using HemdemGames.SplineTool.Behaviours;
using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    public class BezierSplineEditor : ISplineEditor
    {
        private BezierSpline spline;

        public BezierSplineEditor(BezierSpline spline)
        {
            this.spline = spline;
        }

        public int PointCount => spline.Points.Count;

        public void AddPoint(Vector3 position)
        {
            var point = new BezierSplinePoint();
            point.position = position;
            point.SetLeftTangent(Vector3.left);
            point.SetRightTangent(Vector3.right);
            
            bool hasAnyPoint = spline.Points.Count > 0; 
            if (hasAnyPoint)
            {
                BezierSplinePoint previousPoint = spline.Points[spline.Points.Count - 1];
                Vector3 diff = position - previousPoint.position;
            
                point.SetLeftTangent(-diff * .5f);
                point.SetRightTangent(diff * .5f);
            }

            spline.Points.Add(point);
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

            for (int i = 0; i < spline.Points.Count; i++)
            {
                Vector3 newRightTangent = spline.Points[i].leftTangent;
                Vector3 newLeftTangent = spline.Points[i].rightTangent;
                spline.Points[i].SetLeftTangent(newLeftTangent);
                spline.Points[i].SetRightTangent(newRightTangent);
            }
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
            BezierSplinePoint point = spline.Points[index];
            controlId = GUIUtility.GetControlID(point.GetHashCode(), FocusType.Passive);
            Handles.SphereHandleCap(controlId, point.position, Quaternion.identity, SplineSettings.PointRadius, Event.current.type);
        }

        public void DrawPointMoveHandle(int pointIndex, Action onBeforeChange, Action onChanged)
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                BezierSplinePoint point = spline.Points[pointIndex];
                Vector3 position = Handles.PositionHandle(point.position, Quaternion.identity);
                
                if (scope.changed)
                {
                    onBeforeChange?.Invoke();
                    point.position = position;
                    onChanged?.Invoke();
                }
            }
            
            DrawRightTangentMoveHandle(pointIndex, onBeforeChange, onChanged);
            DrawLeftTangentMoveHandle(pointIndex, onBeforeChange, onChanged);
        }

        private void DrawRightTangentMoveHandle(int pointIndex, Action onBeforeChange, Action onChanged)
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                BezierSplinePoint point = spline.Points[pointIndex];
                
                Vector3 rightTangentPos = point.TransformPoint(point.rightTangent);
                rightTangentPos = Handles.PositionHandle(rightTangentPos, Quaternion.identity);
                
                Handles.SphereHandleCap(0, rightTangentPos, Quaternion.identity, SplineSettings.TangentPointRadius, Event.current.type);
                Handles.DrawLine(rightTangentPos, point.position);
                
                if (!scope.changed) return;
                
                onBeforeChange?.Invoke();
                point.SetRightTangent(point.InverseTransformPoint(rightTangentPos));
                onChanged?.Invoke();
            }
        }

        private void DrawLeftTangentMoveHandle(int pointIndex, Action onBeforeChange, Action onChanged)
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                BezierSplinePoint point = spline.Points[pointIndex];
                
                Vector3 leftTangentPos = point.TransformPoint(point.leftTangent);
                leftTangentPos = Handles.PositionHandle(leftTangentPos, Quaternion.identity);
                
                Handles.SphereHandleCap(0, leftTangentPos, Quaternion.identity, SplineSettings.TangentPointRadius, Event.current.type);
                Handles.DrawLine(leftTangentPos, point.position);
                
                if (!scope.changed) return;
                
                onBeforeChange?.Invoke();
                point.SetLeftTangent(point.InverseTransformPoint(leftTangentPos));
                onChanged?.Invoke();
            }
        }
    }
}