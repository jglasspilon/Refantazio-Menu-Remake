using UnityEngine;

public class CharacterSelectionSection_Party : CharacterSelectionSection, IHandlePageLeftLv1, IHandlePageRightLv1
{
    public override void OnConfirm()
    {
        base.OnConfirm();
        PartyData data = ObjectResolver.Instance.Resolve<PartyData>();
        Character selectedCharacter = SelectedObject.Character;
        ECharacterType selectedType = selectedCharacter.CharacterType;
        ECharacterType targetType = selectedType == ECharacterType.Party ? ECharacterType.Reserve : ECharacterType.Party;

        if(selectedType == ECharacterType.Guide)
        {
            return;
        }

        if(selectedType == ECharacterType.Leader)
        {
            //TODO: play modal to display system message can't change leader
            return;
        }
        
        if(targetType == ECharacterType.Party && data.ActivePartyFull)
        {
            //TODO: play modal to display system message party full
            return;
        }

        if (targetType == ECharacterType.Party)
            data.AddActivePartyMember(selectedCharacter);
        else
            data.RemoveActivePartyMember(selectedCharacter);
    }

    public void OnPageLeftLv1()
    {
        SelectedObject.Character.SetCharacterBattlePosition(EBattlePosition.Front);
    }

    public void OnPageRightLv1()
    {
        SelectedObject.Character.SetCharacterBattlePosition(EBattlePosition.Back);
    }
}
