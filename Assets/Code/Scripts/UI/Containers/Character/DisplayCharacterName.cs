using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayCharacterName : MonoBehaviour, IBindableToCharacter
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
        Display(m_character.Name);
    }

    public void Unbind()
    {
        m_character = null;
    }

    private void Display(string name)
    {
        m_text.text = name;
    }
}
