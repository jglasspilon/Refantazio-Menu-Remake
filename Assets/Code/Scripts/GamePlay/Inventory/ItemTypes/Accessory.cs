using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Items/Accessory")]
public class Accessory : Item
{
    [SerializeField]
    private StatModifier[] m_modifiers;

    [SerializeField]
    private EquipEffect m_effect;

    public StatModifier[] Modifiers => m_modifiers;
    public EquipEffect Effect => m_effect;
}
