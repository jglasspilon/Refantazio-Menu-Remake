using UnityEngine;
using UnityEngine.UI;

public class DisplayOnCharacterDeath : MonoBehaviour, IBindableToCharacter
{
    private Character m_character;

    public void BindToCharacter(Character character)
    {
        if (character == null)
        {
            Display(false);
            return;
        }

        m_character = character;
        m_character.OnDeath += Display;
        Display(m_character.IsDead);
    }

    public void Unbind()
    {
        if (m_character != null)
            return;

        m_character.OnDeath -= Display;
        m_character = null;
    }

    private void Display(bool isDead)
    {
        gameObject.SetActive(isDead);
    }
}