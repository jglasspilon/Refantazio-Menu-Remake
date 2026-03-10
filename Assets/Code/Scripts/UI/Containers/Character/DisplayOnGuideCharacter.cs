using UnityEngine;

public class DisplayOnGuideCharacter : MonoBehaviour, IBindableToCharacter
{
    [SerializeField]
    private bool m_invert;

    private Character m_character;

    public void BindToCharacter(Character character)
    {
        if (character == null)
            return;

        m_character = character;
        Display(m_character);
    }

    public void Unbind()
    {
        if (m_character != null)
            return;

        m_character = null;
    }

    private void Display(Character character)
    {
        bool show = character.IsGuide;

        if (m_invert)
            show = !show;

        gameObject.SetActive(show);
    }
}
