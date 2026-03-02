using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SkillsMenuPage : MenuPage
{
    public event Action<Character> OnCasterChange;

    [SerializeField]
    private CharacterSelectionSection_Skills m_characterSelectionSection;

    private Character m_selectedCaster;
    private Skill m_selectedSkill;

    public Character Caster => m_selectedCaster;

    public void ChangeCaster(Character character)
    {
        if (character == null)
            return;

        m_selectedCaster = character;
        OnCasterChange?.Invoke(character);
    }

    public void SelectSkill(Skill skill)
    {
        if (skill == null)
            return;

        m_selectedSkill = skill;
    }

    public void SelectTarget(Character character)
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
