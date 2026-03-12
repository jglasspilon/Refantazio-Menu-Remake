using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Items/Usable Item")]
public class UsableItemData : ItemData
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

    public override Item CreateItemFromData()
    {
        return new UsableItem(ID, Name, SortOrder, Category, Icon, Price, Description, m_battleOnly, m_effects, m_targetingType);
    }
}

public class UsableItem : Item
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

    public UsableItem(string id, string name, int sortOrder, EItemCategories category, Sprite icon, int price, string description, bool battleOnly, ItemEffect[] effects, ETargetingTypes targetingType) : base(id, name, sortOrder, category, icon, price, description)
    {
        m_battleOnly = battleOnly;
        m_effects = effects;
        m_targetingType = targetingType;
    }
}