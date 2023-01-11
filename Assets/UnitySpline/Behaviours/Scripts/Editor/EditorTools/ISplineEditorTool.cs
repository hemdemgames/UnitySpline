using UnityEditor;

namespace HemdemGames.SplineTool.Behaviours
{
    public interface ISplineEditorTool
    {
        string Title { get; }

        void OnEnable();
        
        void OnSceneGUI(SceneView sceneView);
        void OnInspectorGUI();

        void OnDisable();
    }
}