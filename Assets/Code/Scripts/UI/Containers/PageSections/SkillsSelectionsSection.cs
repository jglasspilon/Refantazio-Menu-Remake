using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SkillsSelectionsSection : UIListSelectionSection<MenuSkill, MenuSkillGenerator, Skill, PartyData>, IHandleOnConfirm, IHandleOnBack
{
    [SerializeField]
    private SkillsMenuPage m_parentPage;

    [SerializeField]
    private CharacterSelecter m_casterSelecter;

    [SerializeField]
    private Animator m_sectionAnim;

    private Character m_displayedCaster;
    private Skill m_selectedSkill;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_casterSelecter.OnSelectedObjectChanged += HandleOnDisplayedCasterChanged;
        m_parentPage.OnCasterReleased += HandleOnCasterReleased;

        if (m_selecter is SkillSelecter skillSelecter)
            skillSelecter.OnSkillSelected += HandleOnSkillSelected;
    }

    private void OnDisable()
    {
        m_casterSelecter.OnSelectedObjectChanged -= HandleOnDisplayedCasterChanged;
        m_parentPage.OnCasterReleased -= HandleOnCasterReleased;

        if (m_selecter is SkillSelecter skillSelecter)
            skillSelecter.OnSkillSelected -= HandleOnSkillSelected;
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
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
        m_generater.ClearGeneratedContent();
    }

    protected override void GenerateUIContent()
    {
        if(m_displayedCaster.IsGuide)
        {
            m_generater.ClearGeneratedContent();
            return;
        }

        Skill[] skillsToGenerate = m_displayedCaster == null ? Array.Empty<Skill>() : m_displayedCaster.Skills;
        var generatedItems = m_generater.GenerateContent(skillsToGenerate);
        generatedItems.ForEach(x => x.InjectCharacter(m_displayedCaster));
        m_selecter.UpdateObjects(generatedItems);
    }

    private void HandleOnSkillSelected(Skill skill)
    {
        m_selectedSkill = skill;
    }

    private void HandleOnDisplayedCasterChanged(CharacterBanner newCasterBanner)
    {
        if (m_selectedSkill != null)
            return;

        m_displayedCaster = newCasterBanner.Character;
        GenerateUIContent();
    }

    private void HandleOnCasterReleased()
    {
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
    }
}
