using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayCharacterLevel : MonoBehaviour, IBindableToCharacter
{
    private TextMeshProUGUI m_text;
    private Character m_character;

    private void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();   
    }

    public void BindToCharacter(Character character)
    {
        if (character == null)
            return;

        m_character = character;
        m_character.OnLevelChange += Display;
        Display(m_character.Level.Value, 0);
    }

    public void Unbind()
    {
        if (m_character != null)
            return;

        m_character.OnLevelChange -= Display;
        m_character = null;
    }

    private void Display(int level, int delta)
    {
        m_text.text = level.ToString("00");
    }
}
