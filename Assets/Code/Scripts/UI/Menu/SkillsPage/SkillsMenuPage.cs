using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SkillsMenuPage : MenuPage
{
    public event Action<Character> OnCasterChange;

    [SerializeField]
    private CharacterSelectionSection_Skills m_characterSelectionSection;

    [SerializeField]
    private SkillsSelectionsSection m_skillSelectionSection;

    private Character m_selectedCaster;
    private Skill m_selectedSkill;

    public Character Caster => m_selectedCaster;

    private void Awake()
    {
        m_skillSelectionSection.OnSkillSelected += SelectSkill;
        m_characterSelectionSection.OnCharacterSelected += SelectCaster;
    }

    private void OnDestroy()
    {
        m_skillSelectionSection.OnSkillSelected += SelectSkill;
        m_characterSelectionSection.OnCharacterSelected -= SelectCaster;
    }

    public void ChangeCaster(Character character)
    {
        if (character == null || m_selectedCaster == character)
            return;

        m_selectedCaster = character;
        OnCasterChange?.Invoke(character);
    }

    private void SelectCaster(Character character)
    {
        if (character.Skills.Length == 0)
            return;

        EnterSection(m_skillSelectionSection);
    }

    private void SelectSkill(Skill skill)
    {
        if (skill == null)
            return;

        m_selectedSkill = skill;
    }

    private void SelectTarget(Character character)
    {
        if(character == null) 
            return;

        //Cast the skill
    }

    public override UniTask EnterDefaultSection()
    {
        EnterSection(m_characterSelectionSection);
        return default;
    }
}
