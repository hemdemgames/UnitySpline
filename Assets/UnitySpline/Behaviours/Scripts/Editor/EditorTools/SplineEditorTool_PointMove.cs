using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineEditorPointMoveTool : ISplineEditorTool
    {
        public string Title => "Move";

        private int passiveControlID;
        private SplineComponentEditor splineEditor;

        private SplinePointSelectionManager selectionManager;

        public SplineEditorPointMoveTool(SplineComponentEditor splineEditor)
        {
            this.splineEditor = splineEditor;
            selectionManager = new SplinePointSelectionManager(splineEditor);
            selectionManager.OnSelectionsChanged -= SceneView.RepaintAll;
            selectionManager.OnSelectionsChanged += SceneView.RepaintAll;
        }
        
        public void OnEnable()
        {
            passiveControlID = GUIUtility.GetControlID(FocusType.Passive);
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            HandleUtility.AddDefaultControl(passiveControlID);
            
            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.Escape)
                {
                    selectionManager.ClearSelections();
                    Event.current.Use();
                }
            }

            using (new Handles.DrawingScope(splineEditor.component.transform.localToWorldMatrix))
            {
                selectionManager.OnSceneGUI();

                using (var scope = new EditorGUI.ChangeCheckScope())
                {
                    if (selectionManager.SelectionCount == 1)
                    {
                        SingleSelectionMove(selectionManager.GetPointIndex(0));
                    }
                    else if (selectionManager.SelectionCount > 1)
                    {
                        MultiSelectionMove();
                    }

                    if (scope.changed)
                    {
                        OnSplineChanged();
                    }
                }
            }
            
        }

        private void SingleSelectionMove(int index)
        {
            splineEditor.GetSpline().Editor.DrawPointMoveHandle(index,
                () => { splineEditor.CreateUndo("Spline Point Move");},
                splineEditor.Rebuild);
        }

        private void MultiSelectionMove()
        {
            Vector3 centerPos = selectionManager.GetSelectionsCenter();
            
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                Vector3 newPos = Handles.PositionHandle(centerPos, Quaternion.identity);
                    
                if (scope.changed)
                {
                    splineEditor.CreateUndo("Spline Point Move");
                    Vector3 offset = newPos - centerPos;
                    for (int i = selectionManager.SelectionCount - 1; i >= 0; i--)
                    {
                        int selectedPoint = selectionManager.GetPointIndex(i);
                        Vector3 pointPosition = splineEditor.GetSpline().Editor.GetPointPosition(selectedPoint);
                        splineEditor.GetSpline().Editor.SetPointPosition(selectedPoint, pointPosition + offset);
                    }
                }
            }
        }

        private void OnSplineChanged()
        {
            splineEditor.Rebuild();
        }

        public void OnInspectorGUI()
        {
            
        }
        
        public void OnDisable()
        {
            
        }
    }
}