using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class CharacterSelectionSection: UIListSelectionSection<CharacterBanner, CharacterBannerGenerator, Character, PartyData>
    ,IHandleOnConfirm, IHandleOnBack
{
    [SerializeField]
    private bool m_generateCharactersOnEnable;
    public event Action<Character> OnCharacterSelected;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (m_generateCharactersOnEnable)
            GenerateUIContent();
        else
            m_selecter.UpdateObjectsAndReturnIndex(m_generater.GetGeneratedContent(), m_selectedIndex);
    }
    
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
        m_selectedIndex = 0;
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
