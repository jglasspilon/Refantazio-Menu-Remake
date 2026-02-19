using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(StaggeredVerticalLayoutGroup))]
[CanEditMultipleObjects]
public class StaggeredVerticalByHeightEditor : Editor
{
    // Built-in VerticalLayoutGroup properties
    SerializedProperty padding;
    SerializedProperty childAlignment;
    SerializedProperty spacing;
    SerializedProperty childControlWidth;
    SerializedProperty childControlHeight;
    SerializedProperty childScaleWidth;
    SerializedProperty childScaleHeight;
    SerializedProperty childForceExpandWidth;
    SerializedProperty childForceExpandHeight;
    SerializedProperty reverseArrangement;

    // Your custom property
    SerializedProperty horizontalFactor;

    void OnEnable()
    {
        padding = serializedObject.FindProperty("m_Padding");
        childAlignment = serializedObject.FindProperty("m_ChildAlignment");
        spacing = serializedObject.FindProperty("m_Spacing");

        childControlWidth = serializedObject.FindProperty("m_ChildControlWidth");
        childControlHeight = serializedObject.FindProperty("m_ChildControlHeight");
        childScaleWidth = serializedObject.FindProperty("m_ChildScaleWidth");
        childScaleHeight = serializedObject.FindProperty("m_ChildScaleHeight");
        childForceExpandWidth = serializedObject.FindProperty("m_ChildForceExpandWidth");
        childForceExpandHeight = serializedObject.FindProperty("m_ChildForceExpandHeight");

        reverseArrangement = serializedObject.FindProperty("m_ReverseArrangement");

        horizontalFactor = serializedObject.FindProperty("m_horizontalFactor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // --- Default VerticalLayoutGroup Inspector ---
        EditorGUILayout.PropertyField(padding, true);
        EditorGUILayout.PropertyField(spacing);
        EditorGUILayout.PropertyField(childAlignment);
        
        EditorGUILayout.PropertyField(reverseArrangement);
        EditorGUILayout.LabelField("Child Controls", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(childControlWidth);
        EditorGUILayout.PropertyField(childControlHeight);
        EditorGUILayout.LabelField("Child Scales", EditorStyles.boldLabel); 
        EditorGUILayout.PropertyField(childScaleWidth);
        EditorGUILayout.PropertyField(childScaleHeight);
        EditorGUILayout.LabelField("Child Force Expand", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(childForceExpandWidth);
        EditorGUILayout.PropertyField(childForceExpandHeight);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // --- Your Custom Section ---
        EditorGUILayout.LabelField("Stagger Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(horizontalFactor, new GUIContent("Horizontal Factor"));

        serializedObject.ApplyModifiedProperties();
    }
}