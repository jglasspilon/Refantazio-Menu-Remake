using UnityEngine;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class UniqueScriptableObject : ScriptableObject
{
    protected string m_id = "";

    public string ID => m_id;

#if UNITY_EDITOR
    protected void OnValidate()
    {
        if (string.IsNullOrEmpty(m_id))
        {
            m_id = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }

        var all = AssetDatabase.FindAssets($"t:{GetType().Name}").Select(g => AssetDatabase.LoadAssetAtPath<UniqueScriptableObject>(AssetDatabase.GUIDToAssetPath(g)));

        if (all.Any(a => a != this && a.ID == ID))
        {
            m_id = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }
    }
#endif
}
