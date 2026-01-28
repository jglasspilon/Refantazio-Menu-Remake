using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
[ExecuteAlways]
public class TextMeshGlyphScaler : MonoBehaviour
{
    [SerializeField]
    private Vector2 m_characterScale;

    TMP_Text m_text;

    private void Awake()
    {
        m_text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if(m_text == null)
            m_text = GetComponent<TMP_Text>();

        m_text.OnPreRenderText += OnPreRenderText;

        if (!Application.isPlaying)
            m_text.ForceMeshUpdate();
    }

    private void OnDisable()
    {
        if (m_text != null)
            m_text.OnPreRenderText -= OnPreRenderText;
    }

    private void OnValidate()
    {
        if(m_text == null)
            m_text = GetComponent<TMP_Text>();

        m_text.ForceMeshUpdate();
    }

    private void OnPreRenderText(TMP_TextInfo textInfo)
    {
        TMP_MeshInfo[] meshInfo = textInfo.meshInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            ApplyCharacterScale(meshInfo, charInfo, m_characterScale);
        }
    }

    private void ApplyCharacterScale(TMP_MeshInfo[] meshInfo, TMP_CharacterInfo charInfo, Vector2 scale)
    {
        int vIndex = charInfo.vertexIndex;
        int mIndex = charInfo.materialReferenceIndex;
        Vector3[] verts = meshInfo[mIndex].vertices;

        Vector3 newScale = default;
        newScale.x = scale.x;
        newScale.y = scale.y;
        newScale.z = 1;

        for (int j = 0; j < 4; j++)
        {
            verts[vIndex + j].Scale(newScale);
        }
    }
}
