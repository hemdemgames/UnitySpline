using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineEditorTool_PointAdd : ISplineEditorTool
    {
        public string Title => "Add";

        private int passiveControlID;
        private SplineComponentEditor splineEditor;

        public SplineEditorTool_PointAdd(SplineComponentEditor splineEditor)
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
            
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 0)
                {
                    if (TryGetPlacementPoint(out Vector3 point))
                    {
                        splineEditor.CreateUndo("Spline Add Point");
                        point = splineEditor.component.transform.InverseTransformPoint(point);
                        splineEditor.GetSpline().Editor.AddPoint(point);
                        OnSplineChanged();
                        Event.current.Use();
                    }
                }
            }
            
            for (int i = 0; i < splineEditor.GetSpline().Editor.PointCount; i++)
            {
                using (new Handles.DrawingScope(splineEditor.component.transform.localToWorldMatrix))
                {
                    splineEditor.GetSpline().Editor.DrawPointCap(i, out int controlId);
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
        
        private bool TryGetPlacementPoint(out Vector3 point)
        {
            if (SplineSettings.PlacementMode == PlacementMode.Surface)
            {
                if (SceneViewCameraUtility.Raycast(out RaycastHit hit))
                { 
                    point = hit.point;
                    return true;
                }

                point = Vector3.zero;
                return false;
            }

            Camera camera = SceneView.currentDrawingSceneView.camera;
            float depth = Vector3.Distance(splineEditor.component.transform.position, camera.transform.position);
            point = SceneViewCameraUtility.GetMouseWorldPosition(depth);
            return true;
        }

        public void OnDisable()
        {
            
        }
    }
}