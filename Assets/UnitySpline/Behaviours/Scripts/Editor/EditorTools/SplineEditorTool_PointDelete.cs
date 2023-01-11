using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineEditorPointDeleteTool : ISplineEditorTool
    {
        public string Title => "Delete";

        private int passiveControlID;
        private SplineComponentEditor splineEditor;

        public SplineEditorPointDeleteTool(SplineComponentEditor splineEditor)
        {
            this.splineEditor = splineEditor;
        }
        
        public void OnEnable()
        {
            passiveControlID = GUIUtility.GetControlID(FocusType.Passive);
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            HandleUtility.AddDefaultControl(passiveControlID);
            
            using (new Handles.DrawingScope(Color.red, splineEditor.component.transform.localToWorldMatrix))
            {
                int controlId;
                Vector3 position;
                
                for (int i = 0; i < splineEditor.GetSpline().Editor.PointCount; i++)
                {
                    controlId = splineEditor.GetSpline().Editor.GetPointControlID(i, FocusType.Passive);
                    position = splineEditor.GetSpline().Editor.GetPointPosition(i);
                    
                    Handles.SphereHandleCap(controlId, position, Quaternion.identity, SplineSettings.PointRadius, Event.current.type);
                    
                    if (Event.current.type != EventType.MouseDown) continue;
                    if (Event.current.button != 0) continue;
                    if (HandleUtility.nearestControl != controlId) continue;
                    
                    splineEditor.CreateUndo("Spline Point Remove");
                    splineEditor.GetSpline().Editor.RemovePoint(i);
                    splineEditor.Rebuild();
                }
            }
        }

        public void OnInspectorGUI()
        {
            
        }

        public void OnDisable()
        {
            
        }
    }
}