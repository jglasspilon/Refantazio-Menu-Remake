using TMPro;
using UnityEngine;

public class DisplayCharacterType : MonoBehaviour, IBindableToCharacter
{
    [SerializeField]
    private TextMeshProUGUI m_text;

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
        string typeString = character.CharacterType.ToString();
        string output = $"{typeString.Substring(0, 1)}<size=50%>{typeString.Substring(1)}";
        m_text.text = output;

        gameObject.SetActive(character.IsInActiveParty || character.IsGuide);
    }
}
