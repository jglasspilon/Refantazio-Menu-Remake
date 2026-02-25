using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Items/Equipment")]
public class Equipment : Item
{
    [SerializeField]
    private StatModifier m_mainModifier;

    [SerializeField]
    private StatModifier m_secondaryModifier;

    [SerializeField]
    private StatModifier[] m_modifiers;

    [SerializeField]
    private EquipEffect m_effect;

    public StatModifier MainModifier => m_mainModifier;
    public StatModifier SecondaryModifier => m_secondaryModifier;
    public StatModifier[] Modifiers => m_modifiers;
    public EquipEffect Effect => m_effect;
}
