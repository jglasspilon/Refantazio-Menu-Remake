using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Item Effects/Proportional Fill Mana")]
public class ItemEffect_ProportionalFillMana : ItemEffect
{
    [SerializeField]
    private float m_proportion;

    public override bool CanApply(Character target)
    {
        return target.MP.Current < target.MP.Max && !target.IsDead.Value;
    }

    public override void Apply(Character target)
    {
        int amount = Mathf.CeilToInt(target.MP.Max * m_proportion);
        target.MP.Apply(amount);
    }
}
