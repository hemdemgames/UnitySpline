using System;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    [Serializable]
    public class LinearSpline : Spline
    {
        [SerializeField] private LinearSplinePointCollection points = new();
        
        private LinearSplineGizmo splineGizmo;
        private LinearSplineEditor splineEditor;

        public LinearSplinePointCollection Points => points;

        public override ISplineEditor Editor
        {
            get
            {
                splineEditor ??= new LinearSplineEditor(this);
                return splineEditor;
            }
        }
        
        public override ISplineGizmo Gizmo
        {
            get
            {
                splineGizmo ??= new LinearSplineGizmo(this);
                return splineGizmo;
            }
        }

        protected override SplineBuild Build()
        {
            return new LinearSplineBuilder(this).Build();
        }
    }
}