using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineComponentEditor
    {
        public MonoBehaviour component { get; private set; }
        private SerializedObject serializedBehaviour;
        private SerializedProperty splineProperty;
        private Editor inspector;

        public SplineComponentEditor(MonoBehaviour component, SerializedProperty splineProperty, Editor inspector)
        {
            this.component = component;
            this.inspector = inspector;
            this.splineProperty = splineProperty;
        }

        public Spline GetSpline() => splineProperty.managedReferenceValue as Spline;
        public void SetSpline(Spline spline) => splineProperty.managedReferenceValue = spline;

        public void RepaintInspector() => inspector.Repaint();
        
        public void CreateUndo(string name)
        {
            Undo.RegisterCompleteObjectUndo(component, name);
        }
        
        public void Rebuild()
        {
            GetSpline().Rebuild();
            EditorUtility.SetDirty(component);
        }
    }
}