using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(StaggeredScrollRect))]
[CanEditMultipleObjects]
public class StaggeredScrollRectEditor : Editor
{
    // ScrollRect built‑in properties
    SerializedProperty content;
    SerializedProperty viewport;
    SerializedProperty horizontal;
    SerializedProperty vertical;
    SerializedProperty movementType;
    SerializedProperty elasticity;
    SerializedProperty inertia;
    SerializedProperty decelerationRate;
    SerializedProperty scrollSensitivity;

    SerializedProperty horizontalScrollbar;
    SerializedProperty verticalScrollbar;
    SerializedProperty horizontalScrollbarVisibility;
    SerializedProperty verticalScrollbarVisibility;
    SerializedProperty horizontalScrollbarSpacing;
    SerializedProperty verticalScrollbarSpacing;

    // Your custom property
    SerializedProperty horizontalFactor;

    void OnEnable()
    {
        // Standard ScrollRect properties
        content = serializedObject.FindProperty("m_Content");
        viewport = serializedObject.FindProperty("m_Viewport");
        horizontal = serializedObject.FindProperty("m_Horizontal");
        vertical = serializedObject.FindProperty("m_Vertical");
        movementType = serializedObject.FindProperty("m_MovementType");
        elasticity = serializedObject.FindProperty("m_Elasticity");
        inertia = serializedObject.FindProperty("m_Inertia");
        decelerationRate = serializedObject.FindProperty("m_DecelerationRate");
        scrollSensitivity = serializedObject.FindProperty("m_ScrollSensitivity");

        horizontalScrollbar = serializedObject.FindProperty("m_HorizontalScrollbar");
        verticalScrollbar = serializedObject.FindProperty("m_VerticalScrollbar");
        horizontalScrollbarVisibility = serializedObject.FindProperty("m_HorizontalScrollbarVisibility");
        verticalScrollbarVisibility = serializedObject.FindProperty("m_VerticalScrollbarVisibility");
        horizontalScrollbarSpacing = serializedObject.FindProperty("m_HorizontalScrollbarSpacing");
        verticalScrollbarSpacing = serializedObject.FindProperty("m_VerticalScrollbarSpacing");

        // Custom stagger variable
        horizontalFactor = serializedObject.FindProperty("m_horizontalFactor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // --- ScrollRect default inspector recreation ---

        EditorGUILayout.PropertyField(content);
        EditorGUILayout.PropertyField(viewport);

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Scrolling", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(horizontal);
        EditorGUILayout.PropertyField(vertical);
        EditorGUILayout.PropertyField(movementType);
        EditorGUILayout.PropertyField(elasticity);
        EditorGUILayout.PropertyField(inertia);
        if (inertia.boolValue)
            EditorGUILayout.PropertyField(decelerationRate);
        EditorGUILayout.PropertyField(scrollSensitivity);

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Scrollbars", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(horizontalScrollbar);
        EditorGUILayout.PropertyField(horizontalScrollbarVisibility);
        EditorGUILayout.PropertyField(horizontalScrollbarSpacing);

        EditorGUILayout.PropertyField(verticalScrollbar);
        EditorGUILayout.PropertyField(verticalScrollbarVisibility);
        EditorGUILayout.PropertyField(verticalScrollbarSpacing);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // --- Custom Stagger Settings ---
        EditorGUILayout.LabelField("Stagger Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(horizontalFactor, new GUIContent("Stagger Factor"));

        serializedObject.ApplyModifiedProperties();
    }
}
