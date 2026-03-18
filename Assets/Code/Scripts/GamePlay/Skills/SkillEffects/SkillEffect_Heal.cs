using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Skill Effects/Heal")]
public class SkillEffect_Heal : SkillEffect
{
    [SerializeField]
    private int m_baseAmount;

    [SerializeField]
    private float m_magicFactor;

    public override bool CanApply(Character target)
    {
        return target.HP.Current < target.HP.Max && !target.IsDead.Value;
    }

    public override void Apply(Character target, Character caster)
    {
        if (target.IsDead.Value) 
            return;

        int amount = Mathf.FloorToInt(m_baseAmount * (1f + (caster.Stats.Magic.Final.Value * m_magicFactor / 100f)));
        target.HP.Apply(amount);
    }
}
