using UnityEngine;
using UnityEditor;

public class SpritePivotBatchEditor : EditorWindow
{
    private Vector2 newPivot = new Vector2(0.5f, 0.5f);

    [MenuItem("Tools/Batch Set Sprite Pivot")]
    public static void ShowWindow()
    {
        GetWindow<SpritePivotBatchEditor>("Pivot Batch Setter");
    }

    void OnGUI()
    {
        GUILayout.Label("Set Custom Pivot", EditorStyles.boldLabel);
        newPivot = EditorGUILayout.Vector2Field("New Pivot (0~1)", newPivot);

        if (GUILayout.Button("Apply to Selected Textures"))
        {
            foreach (var obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer != null && importer.textureType == TextureImporterType.Sprite)
                {
                    if (importer.spriteImportMode == SpriteImportMode.Single)
                    {
                        var settings = new TextureImporterSettings();
                        importer.ReadTextureSettings(settings);

                        settings.spriteAlignment = (int)SpriteAlignment.Custom;
                        settings.spritePivot = newPivot;

                        importer.SetTextureSettings(settings);
                        importer.SaveAndReimport();

                        Debug.Log($"Pivot updated: {path}");
                    }
                    else
                    {
                        Debug.LogWarning($"Skipped (not Single): {path}");
                    }
                }
            }
        }
    }
}
