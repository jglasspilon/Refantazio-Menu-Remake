using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Items/Non-usable Item")]
public class Item : UniqueScriptableObject
{
    [SerializeField]
    private string m_name;

    [SerializeField]
    private int m_sortOrder;

    [SerializeField]
    private EItemCategories m_category;

    [SerializeField]
    private Sprite m_icon;

    [SerializeField]
    private int m_price;

    [SerializeField]
    [TextArea]
    private string m_description;

    public string Name => m_name;
    public string Description => m_description;
    public EItemCategories Category => m_category;  
    public int Price => m_price;
    public Sprite Icon => m_icon;

    public int SortOrder => m_sortOrder;
}

[CreateAssetMenu(menuName = "Game Data/Items/Usable Item")]
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
}

[CreateAssetMenu(menuName = "Game Data/Items/Weapon")]
public class Weapon : Item
{

}

[CreateAssetMenu(menuName = "Game Data/Items/Armor")]
public class Armor : Item
{

}

[CreateAssetMenu(menuName = "Game Data/Items/Accessory")]
public class Accessory : Item
{

}

[CreateAssetMenu(menuName = "Game Data/Items/Igniter")]
public class Igniter : Accessory
{

}

public class ItemExecutor
{
    public void Use(InventoryEntry item, Character[] targets)
    {
        if (item.Item is not UsableItem usable)
            return;

        item.ApplyAmount(-1);
        foreach(Character target in targets)
        {
            ApplyEffects(target, usable.Effects);
        }
    }

    public void Use(InventoryEntry item, Character target)
    {
        if (item.Item is not UsableItem usable)
            return;

        item.ApplyAmount(-1);
        ApplyEffects(target, usable.Effects);
    }

    private void ApplyEffects(Character target, ItemEffect[] effects)
    {
        foreach(ItemEffect effect in effects)
        {
            effect.Apply(target);
        }
    }
}

public enum EItemCategories
{
    Usable,
    Valuable,
    KeyItem,
    QuestItem,
    Weapon,
    Armor,
    Appendages,
    Accessory,
    All
}