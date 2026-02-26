using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class CharacterSelectionSection: UIListSelectionSection<PartyBanner, PartyBannerGenerator, Character, PartyData>
    ,IHandleOnConfirm, IHandleOnBack
{
    public event Action<Character> OnCharacterSelected;
    
    [SerializeField]
    private GameObject m_allSelectionSplotch;

    [Header("Animation")][SerializeField]
    private Animator m_sectionAnim;

    [SerializeField]
    private AnimatedMover m_bodyMover;

    private bool m_selectedAll;

    public override UniTask EnterSection()
    {
        if(m_bodyMover != null)
            m_bodyMover.MoveIn();

        m_sectionAnim.SetBool("CharSection", true);
        UpdateSelectedObject();
        m_allSelectionSplotch.SetActive(m_selectedAll);
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selectedIndex = 0;
        m_selectedAll = false;
        UpdateSelectabilityOfContent(x => false);
        m_selecter.UnselectAll();
        m_allSelectionSplotch.SetActive(false);
        return default;
    }

    public override void ResetSection()
    {
        m_generater.ClearGeneratedContent();
    }

    protected override void GenerateUIContent()
    {
        Character[] charactersToGenerate = m_dataModel.GetAllPartyMembers();
        var characterBanners = m_generater.GenerateContent(charactersToGenerate);
        m_selectedIndex = m_selecter.UpdateObjectsAndReturnIndex(characterBanners, m_selectedIndex);
    }

    protected override void UpdateSelectedObject()
    {
        if (m_selectedAll)
            return;

        base.UpdateSelectedObject();
    }

    public void SelectAll()
    {
        m_selectedAll = true;
    }

    public void OnConfirm()
    {
        OnCharacterSelected?.Invoke(SelectedObject.Character);
    }

    public void OnBack()
    {
        m_selectedIndex = 0;
    }  
}
