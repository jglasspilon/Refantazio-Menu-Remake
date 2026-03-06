using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DisplayArchetypeIcon : MonoBehaviour, IBindableToCharacter
{
    private Image m_icon;
    private Character m_character;

    private void Awake()
    {
        m_icon = GetComponent<Image>();
    }

    public void BindToCharacter(Character character)
    {
        if (character == null)
            return;

        m_character = character;
        Display(m_character.Equipment.Archetype?.Icon);
    }

    public void Unbind()
    {
        m_character = null;
    }

    private void Display(Sprite sprite)
    {
        gameObject.SetActive(sprite != null);
        m_icon.sprite = sprite;
    }
}
