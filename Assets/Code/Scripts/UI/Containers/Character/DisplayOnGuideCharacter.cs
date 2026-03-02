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
        m_character.OnTypeChange += Display;
        Display(m_character.CharacterType);
    }

    public void Unbind()
    {
        if (m_character != null)
            return;

        m_character.OnTypeChange -= Display;
        m_character = null;
    }

    private void Display(ECharacterType characterType)
    {
        bool isGuide = characterType == ECharacterType.Guide;

        if (m_invert)
            isGuide = !isGuide;

        gameObject.SetActive(isGuide);
    }
}
