using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSpriteCycler : MonoBehaviour
{
    [SerializeField]
    private Sprite[] m_spritesToCycle;

    [SerializeField]
    private float m_cycleDuration;

    private Image m_image;
    private int m_spriteIndex;
    private float m_cycleTime;

    private void Awake()
    {
        m_image = GetComponent<Image>();
        SetSprite(m_spriteIndex);
    }

    private void Update()
    {
        if(m_cycleTime > m_cycleDuration)
        {
            m_cycleTime = 0;
            ChangeTextureIndex();
            SetSprite(m_spriteIndex);
        }
        else
        {
            m_cycleTime += Time.deltaTime;
        }
    }

    private void ChangeTextureIndex()
    {
        if(m_spriteIndex == m_spritesToCycle.Length - 1)
        {
            m_spriteIndex = 0;
            return;
        }

        m_spriteIndex++;
    }

    private void SetSprite(int spriteIndex)
    {
        if (spriteIndex < 0 || spriteIndex >= m_spritesToCycle.Length)
            return;

        m_image.sprite = m_spritesToCycle[spriteIndex];
    }
}
