using UnityEngine;

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
