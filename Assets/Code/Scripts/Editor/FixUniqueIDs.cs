using UnityEditor;
using UnityEngine;

public static class FixUniqueIDs
{
    [MenuItem("Tools/Regenerate All Unique IDs")]
    public static void Regenerate()
    {
        var guids = AssetDatabase.FindAssets("t:UniqueScriptableObject");

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<UniqueScriptableObject>(path);

            if (asset == null)
                continue;

            // Force OnValidate to run
            EditorUtility.SetDirty(asset);
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Regenerated IDs for {guids.Length} assets.");
    }
}
