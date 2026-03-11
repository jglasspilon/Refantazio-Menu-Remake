using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Item Effects/Revive")]
public class ItemEffect_Revive : ItemEffect
{
    [SerializeField]
    private int m_amount;
    public override bool CanApply(Character target)
    {
        return target.IsDead.Value;
    }

    public override void Apply(Character target)
    {
        target.HP.Apply(m_amount);
    }
}
