using UnityEngine;

public abstract class SkillEffect : UniqueScriptableObject
{
    public abstract void Apply(Character target);
    public abstract bool CanApply(Character character);
}
