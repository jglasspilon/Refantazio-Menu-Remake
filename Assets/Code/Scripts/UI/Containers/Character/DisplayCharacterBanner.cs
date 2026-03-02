using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DisplayCharacterBanner : MonoBehaviour, IBindableToCharacter
{
    private Image m_banner;
    private Character m_character;

    private void Awake()
    {
        m_banner = GetComponent<Image>();    
    }

    public void BindToCharacter(Character character)
    {
        if (character == null)
            return;

        m_character = character;
        Display(m_character.Banner);
    }

    public void Unbind()
    {
        m_character = null;
    }

    private void Display(Sprite sprite)
    {
        m_banner.sprite = sprite;
    }
}
