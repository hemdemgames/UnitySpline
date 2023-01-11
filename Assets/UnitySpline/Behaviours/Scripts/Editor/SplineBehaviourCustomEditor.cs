using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    [CustomEditor(typeof(SplineBehaviour))]
    public class SplineBehaviourCustomEditor : Editor
    {
        private SplineComponentEditor splineComponentEditor;

        private int activeTool = 0;
        private ISplineEditorTool[] tools = new ISplineEditorTool[0];
        private string[] toolNames = new string[0];

        private void OnEnable()
        {
            var splineProperty = serializedObject.FindProperty("spline");
            splineComponentEditor = new SplineComponentEditor(target as SplineBehaviour, splineProperty, this);

            tools = new ISplineEditorTool[]
            {
                new SplineEditorViewerTool(splineComponentEditor),
                new SplineEditorTool_PointAdd(splineComponentEditor),
                new SplineEditorPointDeleteTool(splineComponentEditor),
                new SplineEditorPointMoveTool(splineComponentEditor),
                new SplineEditorPointRotateTool(splineComponentEditor)
            };
            toolNames = tools.Select(tool => tool.Title).ToArray();
            
            SceneView.duringSceneGui -= OnDuringSceneGui;
            SceneView.duringSceneGui += OnDuringSceneGui;
            SceneView.RepaintAll();

        }

        private void OnDuringSceneGui(SceneView sceneView)
        {
            if (splineComponentEditor.GetSpline() != null)
            {
                tools[activeTool].OnSceneGUI(sceneView);
            }
            
            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.Escape)
                {
                    SelectTool(0);
                    Event.current.Use();
                }
            }
        }
        
        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnDuringSceneGui;
        }

        private void SelectTool(int index)
        {
            tools[activeTool].OnDisable();
            activeTool = index;
            tools[activeTool].OnEnable();
            
            OnToolChanged();
        }

        private void OnToolChanged()
        {
            SceneView.RepaintAll();
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawToolbar();

            if (splineComponentEditor.GetSpline() != null)
            {
                tools[activeTool].OnInspectorGUI();
            }

            DrawLoopField();
            OnEditModeInspectorGUI();

            if (GUILayout.Button("Reverse"))
            {
                splineComponentEditor.CreateUndo("Spline Reversing");
                splineComponentEditor.GetSpline().Editor.Reverse();
                splineComponentEditor.Rebuild();
            }
            
            if (splineComponentEditor.GetSpline() is LinearSpline && GUILayout.Button("Convert To Bezier Spline"))
            {
                splineComponentEditor.CreateUndo("Convert to Bezier Spline");
                var linearSpline = splineComponentEditor.GetSpline() as LinearSpline;
                var bezierSpline = SplineConvertUtility.ConvertToBezierSpline(linearSpline);
                splineComponentEditor.SetSpline(bezierSpline);
                splineComponentEditor.Rebuild();
            }

            if (splineComponentEditor.GetSpline() is BezierSpline && GUILayout.Button("Convert To Linear Spline"))
            {
                splineComponentEditor.CreateUndo("Convert to Linear Spline");
                var bezierSpline = splineComponentEditor.GetSpline() as BezierSpline;
                var linearSpline = SplineConvertUtility.ConvertToLinearSpline(bezierSpline);
                splineComponentEditor.SetSpline(linearSpline);
                splineComponentEditor.Rebuild();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawLoopField()
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                var loop = EditorGUILayout.Toggle("Loop", splineComponentEditor.GetSpline().loop);
                
                if (!scope.changed) return;
                
                splineComponentEditor.CreateUndo("Spline Loop");
                splineComponentEditor.GetSpline().SetLoop(loop);
                splineComponentEditor.Rebuild();
            }
        }

        private void DrawToolbar()
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                var selection = GUILayout.Toolbar(activeTool, toolNames);

                if (scope.changed) SelectTool(selection);
            }
        }

        void OnEditModeInspectorGUI()
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                SplineSettings.PlacementMode = (PlacementMode)EditorGUILayout.EnumPopup("Placement Mode", SplineSettings.PlacementMode);
                if (scope.changed) SplineSettings.Save();
            }
            
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                SplineSettings.PointRadius = EditorGUILayout.FloatField("Point Radius", SplineSettings.PointRadius);
                if (scope.changed) SplineSettings.Save();
            }
        }
    }
}