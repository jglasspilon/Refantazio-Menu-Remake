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
            m_selecter.UpdateObjectsAndReturnIndex(m_generater.GetGeneratedContent());
    }
    
    public override UniTask EnterSection()
    {
        m_selecter.SelectCurrent();
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selecter.SetApplicableToSelectable(x => false);
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
        return default;
    }

    public override void ResetSection()
    {
        m_generater.ClearGeneratedContent();
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
    }

    protected override void GenerateUIContent()
    {
        if(m_dataModel == null)
        {
            return;
        }

        Character[] charactersToGenerate = m_dataModel.GetAllPartyMembers();
        var characterBanners = m_generater.GenerateContent(charactersToGenerate);
        m_selecter.UpdateObjectsAndReturnIndex(characterBanners);
    }

    //TODO: apply to selecter
    public override void HandleOnConfirm()
    {
        base.HandleOnConfirm();
        OnCharacterSelected?.Invoke(m_selecter.SelectedObject.Character);
    }
}
