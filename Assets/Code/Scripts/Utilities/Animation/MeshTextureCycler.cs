using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MeshTextureCycler : MonoBehaviour
{
    [SerializeField]
    private Texture[] m_texturesToCycle;

    [SerializeField]
    private string m_texturePropertyName = "_MainTex";

    [SerializeField]
    private float m_cycleDuration;

    private Material m_material;
    private int m_textureIndex;
    private float m_cycleTime;

    private void Awake()
    {
        m_material = GetComponent<MeshRenderer>().material;
        SetMaterialTexture(m_textureIndex);
    }

    private void Update()
    {
        if(m_cycleTime > m_cycleDuration)
        {
            m_cycleTime = 0;
            ChangeTextureIndex();
            SetMaterialTexture(m_textureIndex);
        }
        else
        {
            m_cycleTime += Time.deltaTime;
        }
    }

    private void ChangeTextureIndex()
    {
        if(m_textureIndex == m_texturesToCycle.Length - 1)
        {
            m_textureIndex = 0;
            return;
        }

        m_textureIndex++;
    }

    private void SetMaterialTexture(int textureIndex)
    {
        if (textureIndex < 0 || textureIndex >= m_texturesToCycle.Length)
            return;

        m_material.SetTexture(m_texturePropertyName, m_texturesToCycle[textureIndex]);
    }
}
