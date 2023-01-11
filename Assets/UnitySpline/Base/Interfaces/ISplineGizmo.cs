using UnityEngine;

namespace HemdemGames.SplineTool
{
    public interface ISplineGizmo
    {
        Color Color { get; set; }
        bool DisplayNormals { get; set; }
        bool DisplayArrows { get; set; }
        
        void Draw();
    }
}