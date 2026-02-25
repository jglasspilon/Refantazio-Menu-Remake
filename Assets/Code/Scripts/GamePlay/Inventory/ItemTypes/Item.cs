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