using UnityEngine;

[RequireComponent(typeof(PageSection))]
[DisallowMultipleComponent]
public class PartyFormationSelecter : UIObjectSelecter<CharacterBanner>
{
    private PageSection m_parentSection;

    private void Awake()
    {
        m_parentSection = GetComponent<PageSection>();
        m_parentSection.OnConfirm += SelectCharacter;
        m_parentSection.OnPageRightLv1 += MoveToBack;
        m_parentSection.OnPageLeftLv1 += MoveToFront;
    }

    public void SelectCharacter()
    {
        PartyData data = ObjectResolver.Instance.Resolve((PartyData party) => data = party);
        Character selectedCharacter = SelectedObject.Character;

        if (selectedCharacter.IsGuide)
            return;

        if (!selectedCharacter.IsInActiveParty)
        {
            data.AddActivePartyMember(selectedCharacter);
            return;
        }

        data.RemoveActivePartyMember(selectedCharacter);     
    }

    public void MoveToFront()
    {
        SelectedObject.Character.SetCharacterBattlePosition(EBattlePosition.Front);
    }

    public void MoveToBack()
    {
        SelectedObject.Character.SetCharacterBattlePosition(EBattlePosition.Back);
    }
}
