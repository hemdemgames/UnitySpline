using System.IO;
using UnityEditor;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineSettings : ScriptableObject
    {
        [SerializeField] private float pointRadius = .5f;
        [SerializeField] private float tangentPointRadius = .2f;
        [SerializeField] private PlacementMode placementMode = PlacementMode.CameraView;
        [SerializeField] private Color lineColor = Color.white;
        [SerializeField] private Color normalColor = Color.cyan;
        [SerializeField] private Color arrowColor = Color.white;

        public static float PointRadius
        {
            get => GetInstance().pointRadius;
            set => GetInstance().pointRadius = value;
        }
        
        public static float TangentPointRadius
        {
            get => GetInstance().tangentPointRadius;
            set => GetInstance().tangentPointRadius = value;
        }

        public static PlacementMode PlacementMode
        {
            get => GetInstance().placementMode;
            set => GetInstance().placementMode = value;
        }

        public static Color ArrowColor
        {
            get => GetInstance().arrowColor;
            set => GetInstance().arrowColor = value;
        }
        
        public static Color LineColor
        {
            get => GetInstance().lineColor;
            set => GetInstance().lineColor = value;
        }

        public static Color NormalColor
        {
            get => GetInstance().normalColor;
            set => GetInstance().normalColor = value;
        }
        
        private static SplineSettings instance;
        private static SplineSettings GetInstance()
        {
            instance ??= AssetDatabase.LoadAssetAtPath<SplineSettings>("Assets/Resources/" + nameof(SplineSettings) + ".asset");
            instance ??= CreateSettingsAsset();
            
            return instance;
        }

        private static SplineSettings CreateSettingsAsset()
        {
            string directory = Path.Combine(Application.dataPath, "Resources");
            Directory.CreateDirectory(directory);
            
            SplineSettings asset = CreateInstance<SplineSettings>();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/" + nameof(SplineSettings) + ".asset");
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            return asset;
        }

        public static void Save()
        {
            EditorUtility.SetDirty(GetInstance());
            AssetDatabase.SaveAssetIfDirty(GetInstance());
        }
    }
}