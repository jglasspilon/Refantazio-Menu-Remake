using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Item Effects/Proportional Revive")]
public class ItemEffect_ProportionalRevive : ItemEffect
{
    [SerializeField]
    [Range(0f, 1f)]
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

