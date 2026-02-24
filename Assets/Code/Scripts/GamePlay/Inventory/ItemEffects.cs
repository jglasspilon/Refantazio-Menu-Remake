using UnityEngine;

public abstract class ItemEffect: ScriptableObject
{
    public abstract void Apply(Character target);
    public abstract bool CanApply(Character character);
}

[CreateAssetMenu(menuName = "Game Data/Item Effects/Heal")]
public class ItemEffect_Heal : ItemEffect
{
    [SerializeField]
    private int m_amount;

    public override bool CanApply(Character target)
    {
        return target.HP.Current < target.HP.Max && !target.IsDead;
    }

    public override void Apply(Character target)
    {
        target.HP.Apply(m_amount);
    }    
}

[CreateAssetMenu(menuName = "Game Data/Item Effects/Propotional Heal")]
public class ItemEffect_ProportionalHeal : ItemEffect
{
    [SerializeField]
    private float m_proportion;

    public override bool CanApply(Character target)
    {
        return target.HP.Current < target.HP.Max && !target.IsDead;
    }

    public override void Apply(Character target)
    {
        int amount = Mathf.CeilToInt(target.HP.Max * m_proportion);
        target.HP.Apply(amount);
    }
}

[CreateAssetMenu(menuName = "Game Data/Item Effects/Fill Mana")]
public class ItemEffect_FillMana : ItemEffect
{
    [SerializeField]
    private int m_amount;

    public override bool CanApply(Character target)
    {
        return target.MP.Current < target.MP.Max && !target.IsDead;
    }

    public override void Apply(Character target)
    {
        target.MP.Apply(m_amount);
    }
}

[CreateAssetMenu(menuName = "Game Data/Item Effects/Proportional Fill Mana")]
public class ItemEffect_ProportionalFillMana : ItemEffect
{
    [SerializeField]
    private float m_proportion;

    public override bool CanApply(Character target)
    {
        return target.MP.Current < target.MP.Max && !target.IsDead;
    }

    public override void Apply(Character target)
    {
        int amount = Mathf.CeilToInt(target.MP.Max * m_proportion);
        target.MP.Apply(amount);
    }
}

[CreateAssetMenu(menuName = "Game Data/Item Effects/Revive")]
public class ItemEffect_Revive : ItemEffect
{
    [SerializeField]
    private int m_amount;
    public override bool CanApply(Character target)
    {
        return target.IsDead;
    }

    public override void Apply(Character target)
    {
        target.HP.Apply(m_amount);
    }
}

[CreateAssetMenu(menuName = "Game Data/Item Effects/Proportional Revive")]
public class ItemEffect_ProportionalRevive: ItemEffect
{
    [SerializeField]
    private float m_proportion;

    public override bool CanApply(Character target)
    {
        return target.IsDead;
    }

    public override void Apply(Character target)
    {
        int amount = Mathf.CeilToInt(target.HP.Max * m_proportion);
        target.HP.Apply(amount);
    }
}

