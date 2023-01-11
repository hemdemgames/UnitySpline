using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineEditorViewerTool : ISplineEditorTool
    {
        public string Title => "Viewer";

        private SplineComponentEditor splineEditor;

        public SplineEditorViewerTool(SplineComponentEditor splineEditor)
        {
            this.splineEditor = splineEditor;
        }
        
        public void OnEnable()
        {
            
        }

        public void OnSceneGUI(SceneView sceneView)
        {
            for (int i = 0; i < splineEditor.GetSpline().Editor.PointCount; i++)
            {
                using (new Handles.DrawingScope(splineEditor.component.transform.localToWorldMatrix))
                {
                    splineEditor.GetSpline().Editor.DrawPointCap(i, out int controlId);
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