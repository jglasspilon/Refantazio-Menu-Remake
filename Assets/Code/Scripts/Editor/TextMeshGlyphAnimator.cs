using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextMeshGlyphAnimator))]

public class TextMeshGlyphAnimatorEditor: Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector first
        DrawDefaultInspector();

        TextMeshGlyphAnimator script = (TextMeshGlyphAnimator)target;

        if (!script.IsDriversValid)
        {
            if (GUILayout.Button("Fix Missing Drivers"))
            {
                script.ValidateDrivers();
            }
        }
    }
}
