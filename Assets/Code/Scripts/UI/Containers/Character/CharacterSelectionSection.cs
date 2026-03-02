using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class CharacterSelectionSection: UIListSelectionSection<CharacterBanner, CharacterBannerGenerator, Character, PartyData>
    ,IHandleOnConfirm, IHandleOnBack
{
    public event Action<Character> OnCharacterSelected;
    
    public override UniTask EnterSection()
    {
        UpdateSelectedObject();     
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selectedIndex = 0;
        UpdateSelectabilityOfContent(x => false);
        m_selecter.UnselectAll();       
        return default;
    }

    public override void ResetSection()
    {
        m_generater.ClearGeneratedContent();
    }

    protected override void GenerateUIContent()
    {
        if(m_dataModel == null)
        {
            return;
        }

        Character[] charactersToGenerate = m_dataModel.GetAllPartyMembers();
        var characterBanners = m_generater.GenerateContent(charactersToGenerate);
        m_selectedIndex = m_selecter.UpdateObjectsAndReturnIndex(characterBanners, m_selectedIndex);
    }

    public virtual void OnConfirm()
    {
        OnCharacterSelected?.Invoke(SelectedObject == null ? null : SelectedObject.Character);
    }

    public virtual void OnBack()
    {
        m_selectedIndex = 0;
    }  
}
