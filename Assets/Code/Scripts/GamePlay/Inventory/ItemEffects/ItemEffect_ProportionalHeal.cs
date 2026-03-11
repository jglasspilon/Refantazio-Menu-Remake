using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Item Effects/Propotional Heal")]
public class ItemEffect_ProportionalHeal : ItemEffect
{
    [SerializeField]
    private float m_proportion;

    public override bool CanApply(Character target)
    {
        return target.HP.Current < target.HP.Max && !target.IsDead.Value;
    }

    public override void Apply(Character target)
    {
        if (target.IsDead.Value)
            return;

        int amount = Mathf.CeilToInt(target.HP.Max * m_proportion);
        target.HP.Apply(amount);
    }
}
