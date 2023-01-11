using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineEditorPointRotateTool : ISplineEditorTool
    {
        public string Title => "Rotate";

        private int passiveControlID;
        private SplineComponentEditor splineEditor;
        private SplinePointSelectionManager selectionManager;

        public SplineEditorPointRotateTool(SplineComponentEditor splineEditor)
        {
            this.splineEditor = splineEditor;
            selectionManager = new SplinePointSelectionManager(splineEditor, 1);
            selectionManager.OnSelectionsChanged -= SceneView.RepaintAll;
            selectionManager.OnSelectionsChanged += SceneView.RepaintAll;
            selectionManager.OnSelectionsChanged -= splineEditor.RepaintInspector;
            selectionManager.OnSelectionsChanged += splineEditor.RepaintInspector;
        }
        
        public void OnEnable()
        {
            passiveControlID = GUIUtility.GetControlID(FocusType.Passive);
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            HandleUtility.AddDefaultControl(passiveControlID);
            
            using (new Handles.DrawingScope(splineEditor.component.transform.localToWorldMatrix))
            {
                selectionManager.OnSceneGUI();
            }
        }

        private void OnSplineChanged()
        {
            splineEditor.Rebuild();
        }

        public void OnInspectorGUI()
        {
            if (selectionManager.SelectionCount == 1)
            {
                using (var scope = new EditorGUI.ChangeCheckScope())
                {
                    int pointIndex = selectionManager.GetPointIndex(0);
                    float angle = splineEditor.GetSpline().Editor.GetPointAngle(pointIndex);
                    angle = EditorGUILayout.FloatField("Angle", angle);

                    if (scope.changed)
                    {
                        splineEditor.CreateUndo("Spline Point Angle");
                        splineEditor.GetSpline().Editor.SetPointAngle(pointIndex, angle);
                        splineEditor.Rebuild();
                    }
                }
            }
        }
        
        public void OnDisable()
        {
            
        }
    }
}