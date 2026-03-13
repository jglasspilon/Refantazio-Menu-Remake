using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelectionSection: UIListSelectionSection<CharacterBanner, CharacterBannerGenerator, Character, PartyData>
    ,IHandleOnConfirm, IHandleOnBack
{
    [SerializeField] private bool m_generateCharactersOnEnable;
    [SerializeField] private bool m_resetSelectionOnExit = true;
    [Space]
    [SerializeField] private UnityEvent m_onEnter;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (m_generateCharactersOnEnable)
            GenerateUIContent();
        else
            m_selecter.UpdateObjects(m_generater.GetGeneratedContent());
    }
    
    public override UniTask EnterSection()
    {
        m_selecter.SelectCurrent();
        m_onEnter?.Invoke();
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selecter.SetApplicableToSelectable(x => false);
        m_selecter.UnselectAll();

        if(m_resetSelectionOnExit)
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
        m_selecter.UpdateObjects(characterBanners);
    }
}
