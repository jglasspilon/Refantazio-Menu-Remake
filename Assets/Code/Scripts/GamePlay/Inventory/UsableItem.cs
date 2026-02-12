using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Items/Usable Item")]
public class UsableItem: Item
{
    [SerializeField]
    private bool m_battleOnly;

    [SerializeField]
    private ItemEffect[] m_effects;

    [SerializeField]
    private ETargetingTypes m_targetingType;

    public bool BattleOnly => m_battleOnly;
    public ItemEffect[] Effects => m_effects;
    public ETargetingTypes TargetingType => m_targetingType;
}
