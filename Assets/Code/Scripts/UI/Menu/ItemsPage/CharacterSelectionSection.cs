using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class CharacterSelectionSection: UIObjectSelectionSection<PartyBanner, PartyBannerGenerator, Character, PartyData>
    ,IHandleOnConfirm, IHandleOnBack
{
    [SerializeField]
    private GameObject m_allSelectionSplotch;

    [SerializeField]
    private Animator m_sectionAnim;

    [SerializeField]
    private AnimatedMover m_bodyMover;

    private ICharacterSelectable m_parentPage;
    private bool m_selectedAll;

    private void Awake()
    {
        m_parentPage = GetComponentInParent<ICharacterSelectable>();
    }

    public override UniTask EnterSection()
    {
        m_sectionAnim.SetBool("CharSection", true);
        m_bodyMover.MoveIn();
        UpdateSelectedObject();
        m_allSelectionSplotch.SetActive(m_selectedAll);
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selectedIndex = 0;
        m_selectedAll = false;
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
        m_parentPage.SelectCharacter(m_selecter.SelectedObject.Character);
    }

    public void OnBack()
    {
        m_selectedIndex = 0;
    }  
}
