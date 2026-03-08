using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SkillsSelectionsSection : UIListSelectionSection<MenuSkill, MenuSkillGenerator, Skill, PartyData>, IHandleOnConfirm, IHandleOnBack
{
    public event Action<Skill> OnSkillSelected;

    [SerializeField]
    private SkillsMenuPage m_parentPage;

    [SerializeField]
    private Animator m_sectionAnim;

    private Character m_displayedCaster;
    private Skill m_selectedSkill;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_parentPage.OnDisplayedCharacterChange += HandleOnDisplayedCasterChanged;
        m_parentPage.OnCasterReleased += HandleOnCasterReleased;
    }

    private void OnDisable()
    {
        m_parentPage.OnDisplayedCharacterChange -= HandleOnDisplayedCasterChanged;
        m_parentPage.OnCasterReleased -= HandleOnCasterReleased;
    }

    public override UniTask EnterSection()
    {
        m_selectedSkill = null;
        m_selecter.SelectCurrent();
        m_sectionAnim.SetBool("CharSection", false);
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selecter.SelectedObject.PauseSelection();
        m_sectionAnim.SetBool("CharSection", true);
        return default;
    }

    public override void ResetSection()
    {
        m_selecter.ResetSelecter();
    }

    public override void HandleOnConfirm()
    {
        m_selectedSkill = m_selecter.SelectedObject.Skill;
        OnSkillSelected?.Invoke(m_selectedSkill);
    }

    public override void HandleOnBack()
    {
        OnSkillSelected?.Invoke(null);
    }

    protected override void GenerateUIContent()
    {
        Skill[] skillsToGenerate = m_displayedCaster == null ? Array.Empty<Skill>() : m_displayedCaster.Skills;
        var generatedItems = m_generater.GenerateContent(skillsToGenerate);
        generatedItems.ForEach(x => x.InjectCharacter(m_displayedCaster));
        m_selecter.UpdateObjectsAndReturnIndex(generatedItems);
    }

    private void HandleOnDisplayedCasterChanged(Character newCaster)
    {
        if (m_selectedSkill != null)
            return;

        m_displayedCaster = newCaster;
        GenerateUIContent();
    }

    private void HandleOnCasterReleased()
    {
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
    }
}
