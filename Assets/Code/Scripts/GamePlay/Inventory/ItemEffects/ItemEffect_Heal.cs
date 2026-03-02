using UnityEngine;

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
        if (target.IsDead) 
            return;

        target.HP.Apply(m_amount);
    }
}
