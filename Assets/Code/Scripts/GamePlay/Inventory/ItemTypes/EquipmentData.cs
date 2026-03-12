using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Items/Equipment")]
public class EquipmentData : ItemData
{
    [SerializeField]
    private EEquipmentType m_equipType;

    [SerializeField]
    private StatModifier m_mainModifier;

    [SerializeField]
    private StatModifier m_secondaryModifier;

    [SerializeField]
    private StatModifier[] m_modifiers;

    [SerializeField]
    private EquipEffectData m_effect;

    public EEquipmentType EquipType => m_equipType;
    public StatModifier MainModifier => m_mainModifier;
    public StatModifier SecondaryModifier => m_secondaryModifier;
    public StatModifier[] Modifiers => m_modifiers;
    public EquipEffectData Effect => m_effect;

    public override Item CreateItemFromData()
    {
        EquipEffect effect = m_effect == null ? null : m_effect.CreateEquipEffectFromData();
        Item item = new Equipment(ID, Name, SortOrder, Category, Icon, Price, Description, m_equipType, m_mainModifier, m_secondaryModifier, m_modifiers, effect);
        item.InitializeProperties();
        return item;
    }
}

public class Equipment : Item
{
    [SerializeField] private EEquipmentType m_equipType;
    [SerializeField] private StatModifier m_mainModifier;
    [SerializeField] private StatModifier m_secondaryModifier;
    [SerializeField] private StatModifier[] m_modifiers;
    [SerializeField] private EquipEffect m_effect;
    [SerializeField] private ObservableProperty<bool> m_hasEffect = new ObservableProperty<bool>();

    public EEquipmentType EquipType => m_equipType;
    public StatModifier MainModifier => m_mainModifier;
    public StatModifier SecondaryModifier => m_secondaryModifier;
    public StatModifier[] Modifiers => m_modifiers;
    public EquipEffect Effect => m_effect;

    public Equipment(string id, string name, int sortOrder, EItemCategories category, Sprite icon, int price, string description, EEquipmentType equipType, StatModifier mainModifier, StatModifier secondaryModifier, StatModifier[] modifiers, EquipEffect effect) : base(id, name, sortOrder, category, icon, price, description)
    {
        m_equipType = equipType;
        m_mainModifier = mainModifier;
        m_secondaryModifier = secondaryModifier;
        m_modifiers = modifiers;
        m_hasEffect.Value = effect != null;
        m_effect = effect == null ? new EquipEffect("") : effect;
    }
}

public enum EEquipmentType
{
    Sword,
    Staff,
    Greatsword,
    Lance,
    Knuckles,
    Mace,
    Crossbow,
    Katana,
    Daggers,
    Semaphore,
    Abacus,
    Fan,
    Croisier,
    Axe,

    Chainmail,
    Robes,
    Clothing,

    Headgear,
    Gloves,
    Shoes,

    Amulet,
    Igniter,
    Mask,
    Trinket
}
