using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public class RotateTextMeshOnly : MonoBehaviour
{
    public float rotationDegrees = 15f;
    TMP_Text text;

    void OnEnable()
    {
        text = GetComponent<TMP_Text>();
        text.OnPreRenderText += OnPreRenderText;

        // Force a mesh update in Edit Mode
        if (!Application.isPlaying)
            text.ForceMeshUpdate();
    }

    void OnDisable()
    {
        if (text != null)
            text.OnPreRenderText -= OnPreRenderText;
    }

    void OnValidate()
    {
        if (text == null) text = GetComponent<TMP_Text>();
        text.ForceMeshUpdate();
    }

    void OnPreRenderText(TMP_TextInfo textInfo)
    {
        var meshInfo = textInfo.meshInfo;
        Quaternion rot = Quaternion.Euler(0, 0, rotationDegrees);

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vIndex = charInfo.vertexIndex;
            int mIndex = charInfo.materialReferenceIndex;

            Vector3[] verts = meshInfo[mIndex].vertices;

            Vector3 mid = (verts[vIndex] + verts[vIndex + 2]) * 0.5f;

            for (int j = 0; j < 4; j++)
                verts[vIndex + j] = rot * (verts[vIndex + j] - mid) + mid;
        }
    }
}