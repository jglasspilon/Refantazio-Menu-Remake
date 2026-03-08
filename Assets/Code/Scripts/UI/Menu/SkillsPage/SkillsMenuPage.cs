using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class SkillsMenuPage : MenuPage
{
    public event Action OnCasterReleased;

    [SerializeField]
    private CharacterSelectionSection m_casterSelectionSection, m_targetSelectionSection;

    [SerializeField]
    private SkillsSelectionsSection m_skillSelectionSection;

    [SerializeField]
    private CharacterSelecter m_casterSelecter, m_targetSelecter;

    [SerializeField]
    private SkillSelecter m_skillSelecter;

    private Character m_selectedCaster;
    private Skill m_selectedSkill;
    private SkillExecutor m_skillExecutor = new SkillExecutor();

    public Character Caster => m_selectedCaster;

    private void Awake()
    {
        m_skillSelecter.OnSkillSelected += SelectSkill;
        m_casterSelecter.OnCharacterSelected += SelectCaster;
        m_targetSelecter.OnCharacterSelected += SelectTarget;
    }

    private void OnDestroy()
    {
        m_skillSelecter.OnSkillSelected -= SelectSkill;
        m_casterSelecter.OnCharacterSelected -= SelectCaster;
        m_targetSelecter.OnCharacterSelected -= SelectTarget;
    }

    private void SelectCaster(Character character)
    {
        if (character.Skills.Length == 0)
            return;

        m_selectedCaster = character;
        EnterSection(m_skillSelectionSection);
    }

    public void ReleaseCaster()
    {
        OnCasterReleased?.Invoke();
    }

    private void SelectSkill(Skill skill)
    {
        if (skill == null || !skill.UsableInMenu || !m_selectedCaster.HasEnoughMana(skill.ManaCost))
            return;

        m_selectedSkill = skill;     
        m_targetSelecter.SetApplicableToSelectable(banner => skill.Effects.Any(effect => effect.CanApply(banner.Character)));
        
        if(m_selectedSkill.TargetingType == ETargetingTypes.AllAllies)
            m_targetSelecter.SelectAll();

        EnterSection(m_targetSelectionSection);
    }

    private void SelectTarget(Character character)
    {
        CastSkill(character, m_selectedSkill, m_selectedCaster);    
        m_targetSelecter.SetApplicableToSelectable(banner => m_selectedSkill.Effects.Any(effect => effect.CanApply(banner.Character)));

        if (!m_selectedCaster.HasEnoughMana(m_selectedSkill.ManaCost))
        {
            TryExitCurrentSection();
        }
    }

    private void CastSkill(Character character, Skill skill, Character caster)
    {
        if (skill.TargetingType == ETargetingTypes.SingleAlly)
        {
            Logger.Log($"Using {skill.Name} on {character.Name}", m_logProfile);
            m_skillExecutor.Cast(skill, character, caster);
        }
        else
        {
            Logger.Log($"Using {skill.Name} on all party members", m_logProfile);
            m_skillExecutor.Cast(skill, ObjectResolver.Instance.Resolve<PartyData>().GetAllPartyMembers(), caster);
        }
    }
}
