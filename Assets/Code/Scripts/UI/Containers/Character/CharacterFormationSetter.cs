using UnityEngine;

public class CharacterFormationSetter : MonoBehaviour
{
    [SerializeField] private PageSection m_parentSection;

    private Character m_selectedCharacter;

    protected void OnEnable()
    {
        m_parentSection.OnPageRightLv1.AddListener(MoveToBack);
        m_parentSection.OnPageLeftLv1.AddListener(MoveToFront);
    }

    private void OnDisable()
    {
        m_parentSection.OnPageRightLv1.RemoveListener(MoveToBack);
        m_parentSection.OnPageLeftLv1.RemoveListener(MoveToFront);
    }

    public void PartySelection()
    {
        if (m_selectedCharacter == null)
            return;

        PartyData data = ObjectResolver.Instance.Resolve((PartyData party) => data = party);

        if (m_selectedCharacter.IsGuide)
            return;

        if (!m_selectedCharacter.IsInActiveParty)
        {
            data.AddActivePartyMember(m_selectedCharacter);
            return;
        }

        data.RemoveActivePartyMember(m_selectedCharacter);
    }

    public void MoveToFront()
    {
        if (m_selectedCharacter == null)
            return;

        m_selectedCharacter.SetCharacterBattlePosition(EBattlePosition.Front);
    }

    public void MoveToBack()
    {
        if (m_selectedCharacter == null)
            return;

        m_selectedCharacter.SetCharacterBattlePosition(EBattlePosition.Back);
    }

    public void HandleOnCharacterChange(Character character)
    {
        m_selectedCharacter = character;
    }
}
