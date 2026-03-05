using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;

public class SkillsMenuPage : MenuPage
{
    public event Action<Character> OnDisplayedCharacterChange;
    public event Action OnCasterReleased;

    [SerializeField]
    private CharacterSelectionSection m_casterSelectionSection, m_targetSelectionSection;

    [SerializeField]
    private SkillsSelectionsSection m_skillSelectionSection;

    private Character m_selectedCaster;
    private Skill m_selectedSkill;
    private SkillExecutor m_skillExecutor = new SkillExecutor();

    public Character Caster => m_selectedCaster;

    private void Awake()
    {
        m_casterSelectionSection.OnSelectedObjectChanged += HandleOnCharacterToDisplayChanged;
        m_targetSelectionSection.OnSelectedObjectChanged += HandleOnCharacterToDisplayChanged;
        m_skillSelectionSection.OnSkillSelected += SelectSkill;
        m_casterSelectionSection.OnCharacterSelected += SelectCaster;
        m_targetSelectionSection.OnCharacterSelected += SelectTarget;
    }

    private void OnDestroy()
    {
        m_casterSelectionSection.OnSelectedObjectChanged -= HandleOnCharacterToDisplayChanged;
        m_targetSelectionSection.OnSelectedObjectChanged -= HandleOnCharacterToDisplayChanged;
        m_skillSelectionSection.OnSkillSelected -= SelectSkill;
        m_casterSelectionSection.OnCharacterSelected -= SelectCaster;
        m_targetSelectionSection.OnCharacterSelected -= SelectTarget;
    }

    public void HandleOnCharacterToDisplayChanged(CharacterBanner characterBanner)
    { 
        if (characterBanner == null || characterBanner.Character == null || m_selectedCaster == characterBanner.Character)
            return;

        m_selectedCaster = characterBanner.Character;
        OnDisplayedCharacterChange?.Invoke(characterBanner.Character);
    }

    public void ReleaseCaster()
    {
        OnCasterReleased?.Invoke();
    }

    private void SelectCaster(Character character)
    {
        if (character.Skills.Length == 0)
            return;

        EnterSection(m_skillSelectionSection);
    }

    private void SelectSkill(Skill skill)
    {
        if (skill == null || !skill.UsableInMenu || m_selectedCaster.MP.Current < skill.ManaCost)
            return;

        m_selectedSkill = skill;
        m_targetSelectionSection.UpdateSelectabilityOfContent(banner => skill.Effects.Any(effect => effect.CanApply(banner.Character)));
        EnterSection(m_targetSelectionSection);
    }

    private void SelectTarget(Character character)
    {
        if(character == null) 
            return;

        m_skillExecutor.Cast(m_selectedSkill, character, m_selectedCaster);
        m_targetSelectionSection.UpdateSelectabilityOfContent(banner => m_selectedSkill.Effects.Any(effect => effect.CanApply(banner.Character)));

        if (m_selectedCaster.MP.Current < m_selectedSkill.ManaCost)
        {
            TryExitCurrentSection();
        }
    }

    public override UniTask EnterDefaultSection()
    {
        EnterSection(m_casterSelectionSection);
        return default;
    }
}
