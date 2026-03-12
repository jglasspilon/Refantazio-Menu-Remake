using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Items/Non-usable Item")]
public class ItemData : UniqueScriptableObject
{
    [SerializeField] protected string m_name;
    [SerializeField] protected int m_sortOrder;
    [SerializeField] protected EItemCategories m_category;
    [SerializeField] protected Sprite m_icon;
    [SerializeField] protected int m_price;  
    [TextArea]
    [SerializeField] protected string m_description;

    public string Name => m_name;
    public string Description => m_description;
    public EItemCategories Category => m_category;  
    public int Price => m_price;
    public Sprite Icon => m_icon;
    public int SortOrder => m_sortOrder;

    public virtual Item CreateItemFromData()
    {
        return new Item(m_id, m_name, m_sortOrder, m_category, m_icon, m_price, m_description);
    }
}

public class Item
{
    [SerializeField] protected string m_name;
    [SerializeField] protected int m_sortOrder;
    [SerializeField] protected EItemCategories m_category;
    [SerializeField] protected Sprite m_icon;
    [SerializeField] protected int m_price;
    [TextArea]
    [SerializeField] protected string m_description;

    private string m_id;

    public string ID => m_id;
    public string Name => m_name;
    public string Description => m_description;
    public EItemCategories Category => m_category;
    public int Price => m_price;
    public Sprite Icon => m_icon;
    public int SortOrder => m_sortOrder;

    public Item(string id, string name, int sortOrder, EItemCategories category, Sprite icon, int price, string description)
    {
        m_id = id;
        m_name = name;
        m_sortOrder = sortOrder;
        m_category = category;
        m_icon = icon;
        m_price = price;
        m_description = description;
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
    Gear,
    Accessory,
    All
}