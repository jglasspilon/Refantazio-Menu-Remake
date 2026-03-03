using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SkillsSelectionsSection : UIListSelectionSection<MenuSkill, MenuSkillGenerator, Skill, PartyData>, IHandleOnConfirm, IHandleOnBack
{
    public event Action<Skill> OnSkillSelected;

    [SerializeField]
    private SkillsMenuPage m_parentPage;

    private Character m_caster;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_parentPage.OnCasterChange += HandleOnCasterChanged;
    }

    private void OnDisable()
    {
        m_parentPage.OnCasterChange -= HandleOnCasterChanged;
    }

    public override UniTask EnterSection()
    {
        m_selectedIndex = m_selecter.Select(m_selectedIndex);
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selectedIndex = 0;
        m_selecter.UnselectAll();
        return default;
    }

    public override void ResetSection()
    {
        m_selectedIndex = 0;
    }

    public void OnConfirm()
    {
        OnSkillSelected?.Invoke(SelectedObject.Skill);
    }

    public void OnBack()
    {
        OnSkillSelected?.Invoke(null);
    }

    protected override void GenerateUIContent()
    {
        Skill[] skillsToGenerate = m_caster == null ? Array.Empty<Skill>() : m_caster.Skills;
        var generatedItems = m_generater.GenerateContent(skillsToGenerate);
        m_selectedIndex = m_selecter.UpdateObjectsAndReturnIndex(generatedItems, m_selectedIndex);
    }

    private void HandleOnCasterChanged(Character newCaster)
    {
        m_caster = newCaster;
        GenerateUIContent();
    }
}
