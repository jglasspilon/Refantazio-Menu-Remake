using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Items/Equipment")]
public class Equipment : Item
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
    private EquipEffect m_effect;

    public EEquipmentType EquipType => m_equipType;
    public StatModifier MainModifier => m_mainModifier;
    public StatModifier SecondaryModifier => m_secondaryModifier;
    public StatModifier[] Modifiers => m_modifiers;
    public EquipEffect Effect => m_effect;
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
