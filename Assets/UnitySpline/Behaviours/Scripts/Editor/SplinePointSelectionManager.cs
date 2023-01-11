using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplinePointSelectionManager
    {
        private int selectionLimit = int.MaxValue;
        private SplineComponentEditor spline;

        public SplinePointSelectionManager(SplineComponentEditor splineEditor) : this(splineEditor, int.MaxValue)
        {
            
        }
        
        public SplinePointSelectionManager(SplineComponentEditor spline, int selectionLimit)
        {
            this.spline = spline;
            this.selectionLimit = selectionLimit;
        }

        private List<int> selectedPoints = new List<int>();

        public event Action OnSelectionsChanged; 

        public void ClearSelections() => selectedPoints.Clear();
        public int SelectionCount => selectedPoints.Count;
        public int GetPointIndex(int selectionIndex) => selectedPoints[selectionIndex];
        
        public void OnSceneGUI()
        {
            for (int i = 0; i < spline.GetSpline().Editor.PointCount; i++)
            {
                bool isSelected = selectedPoints.Contains(i);

                using (new Handles.DrawingScope(isSelected ? Color.blue : Color.white))
                {
                    spline.GetSpline().Editor.DrawPointCap(i, out int controlId);
                    
                    if (Event.current.type != EventType.MouseDown) continue;
                    if (Event.current.button != 0) continue;
                    if (HandleUtility.nearestControl != controlId) continue;

                    if (isSelected)
                    {
                        selectedPoints.Remove(i);
                    }
                    else
                    {
                        if (SelectionCount >= selectionLimit)
                        {
                            selectedPoints.RemoveAt(0);
                        }
                        
                        selectedPoints.Add(i);
                    }
                    
                    OnSelectionsChanged?.Invoke();
                }
            }
        }

        public Vector3 GetSelectionsCenter()
        {
            Vector3 positionSums = Vector3.zero;
            
            for (int i = SelectionCount - 1; i >= 0; i--)
            {
                positionSums += spline.GetSpline().Editor.GetPointPosition(selectedPoints[i]);
            }

            return positionSums / SelectionCount;
        }
    }
}