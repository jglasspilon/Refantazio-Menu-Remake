using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Items/Accessory")]
public class AccessoryData : ItemData
{
    [SerializeField]
    private StatModifier[] m_modifiers;

    [SerializeField]
    private EquipEffect m_effect;

    public StatModifier[] Modifiers => m_modifiers;
    public EquipEffect Effect => m_effect;

    public override Item CreateItemFromData()
    {
        return new Accessory(ID, Name, SortOrder, Category, Icon, Price, Description, m_modifiers, m_effect);
    }
}

public class Accessory : Item
{
    [SerializeField]
    private StatModifier[] m_modifiers;

    [SerializeField]
    private EquipEffect m_effect;

    public StatModifier[] Modifiers => m_modifiers;
    public EquipEffect Effect => m_effect;

    public Accessory(string id, string name, int sortOrder, EItemCategories category, Sprite icon, int price, string description, StatModifier[] modifiers, EquipEffect effect) : base(id, name, sortOrder, category, icon, price, description)
    {
        m_modifiers = modifiers;
        m_effect = effect;
    }
}
