using UnityEngine;

public abstract class SkillEffect : UniqueScriptableObject
{
    public abstract void Apply(Character target, Character caster);
    public abstract bool CanApply(Character character);
}
