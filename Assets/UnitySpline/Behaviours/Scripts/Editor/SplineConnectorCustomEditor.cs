using System;
using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    [CustomEditor(typeof(SplineConnector))]
    public class SplineConnectorCustomEditor : Editor
    {
        private SplineConnector connector;
        private SplineComponentEditor splineEditor;
        private SplineEditorPointMoveTool moveTool;

        private void OnEnable()
        {
            connector = target as SplineConnector;

            var splineProperty = serializedObject.FindProperty("spline");
            splineEditor = new SplineComponentEditor(target as SplineConnector, splineProperty, this);
            moveTool = new SplineEditorPointMoveTool(splineEditor);

            SceneView.duringSceneGui -= OnDuringSceneGui;
            SceneView.duringSceneGui += OnDuringSceneGui;
            SceneView.RepaintAll();
        }

        private void OnDuringSceneGui(SceneView sceneView)
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                moveTool.OnSceneGUI(sceneView);
                
                if (scope.changed) RebuildConnector();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Update Connection"))
            {
                RebuildConnector();
            }
        }

        private void RebuildConnector()
        {
            connector.UpdateConnection();
            splineEditor.Rebuild();
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnDuringSceneGui;
        }
    }
}