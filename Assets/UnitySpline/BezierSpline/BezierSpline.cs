using System;
using System.Collections.Generic;
using HemdemGames.SplineTool.Behaviours;
using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool
{
    [Serializable]
    public class BezierSpline : Spline
    {
        [SerializeField] private BezierSplinePointCollection points = new();

        private BezierSplineGizmo splineGizmo;
        private BezierSplineEditor splineEditor;

        public BezierSplinePointCollection Points => points;

        public override ISplineEditor Editor
        {
            get
            {
                splineEditor ??= new BezierSplineEditor(this);
                return splineEditor;
            }
        }
        
        public override ISplineGizmo Gizmo
        {
            get
            {
                splineGizmo ??= new BezierSplineGizmo(this);
                return splineGizmo;
            }
        }
        
        protected override SplineBuild Build()
        {
            return new BezierSplineBuilder(this).Build();
        }
    }
}