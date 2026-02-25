using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public abstract void Apply(Character target);
    public abstract bool CanApply(Character character);
}
