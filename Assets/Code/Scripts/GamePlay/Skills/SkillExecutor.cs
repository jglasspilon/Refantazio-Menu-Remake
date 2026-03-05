using System.Linq;

public class SkillExecutor
{
    public void Cast(Skill skill, Character target, Character caster)
    {
        if (skill == null || caster.MP.Current < skill.ManaCost)
            return;

        if (!skill.Effects.Any(x => x.CanApply(target)))
            return;

        caster.MP.Apply(-skill.ManaCost);
        ApplyEffects(target, skill.Effects, caster);
    }

    public void Cast(Skill skill, Character[] targets, Character caster)
    {
        if (skill == null || caster.MP.Current < skill.ManaCost)
            return;

        if (!targets.Any(c => skill.Effects.Any(x => x.CanApply(c))))
            return;

        caster.MP.Apply(-skill.ManaCost);
        foreach (Character target in targets)
        {
            ApplyEffects(target, skill.Effects, caster);
        }
    }

    private void ApplyEffects(Character target, SkillEffect[] effects, Character caster)
    {
        foreach (SkillEffect effect in effects)
        {
            effect.Apply(target, caster);
        }
    }
}
