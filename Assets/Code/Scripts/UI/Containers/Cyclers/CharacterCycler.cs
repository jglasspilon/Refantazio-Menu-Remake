using UnityEngine;

public class CharacterCycler : MonoBehaviour
{
    [SerializeField] private PageSection m_parentSection;
    [SerializeField] private CharacterSelecter m_characterSelecter;

    private void OnEnable()
    {
        m_parentSection.OnPageLeftLv1 += PreviousCharacter;
        m_parentSection.OnPageRightLv1 += NextCharacter;
    }

    private void OnDisable()
    {
        m_parentSection.OnPageLeftLv1 -= PreviousCharacter;
        m_parentSection.OnPageRightLv1 -= NextCharacter;
    }

    private void NextCharacter()
    {
        m_characterSelecter.SelectNext();
    }

    private void PreviousCharacter()
    {
        m_characterSelecter.SelectPrevious();
    }
}
