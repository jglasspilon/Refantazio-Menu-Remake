using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentExecutor: ISubPropertyProvider
{
    [SerializeField]
    private Archetype m_archetype;

    [SerializeField]
    private Equipment m_weapon, m_armor, m_gear;

    [SerializeField]
    private Equipment m_accessorySlot;

    [NonSerialized]
    private Character m_equiper;

    public event Action OnEquipmentChanged;

    public Archetype Archetype => m_archetype;
    public Equipment Weapon => m_weapon;
    public Equipment Armor => m_armor;
    public Equipment Gear => m_gear;
    public Equipment Accessory => m_accessorySlot;

    public EquipmentExecutor(Equipment weapon, Equipment armor, Equipment gear, Equipment accessory, Archetype startingArchetype, Character equiper)
    {
        m_equiper = equiper;
        EquipArchetype(startingArchetype);
        EquipWeapon(weapon);
        EquipArmor(armor);
        EquipGear(gear);
        EquipAccessory(accessory);
    }

    public IEnumerable<KeyValuePair<string, IObservableProperty>> GetSubProperties(string parentKey)
    {
        return Helper.DataHandling.GetObservableFields(this, parentKey);
    }

    public void EquipArchetype(Archetype archetype)
    {
        if (archetype == null)
            return;
        if (m_archetype != null)
        {
            m_archetype.StatModifiers.ForEach(x => m_equiper.Stats.GetStat(x.Type).RemoveModifier(x));
            m_archetype.OnStatModifiersChanged -= HandleArchetypeStatModifierChange;
        }

        m_archetype = archetype;
        m_archetype.StatModifiers.ForEach(x => m_equiper.Stats.GetStat(x.Type).AddModifier(x));
        m_archetype.OnStatModifiersChanged += HandleArchetypeStatModifierChange;
        EquipWeapon(m_archetype.DefaultWeapon);
    }

    public void EquipWeapon(Equipment weapon)
    {
        if (weapon == null)
            return;

        if (m_archetype != null && m_archetype.EquipableWeaponType != weapon.EquipType)
            return;

        ChangeEquipment(weapon, m_weapon);
        m_weapon = weapon;
        OnEquipmentChanged?.Invoke();
    }

    public void EquipArmor(Equipment armor)
    {
        if (armor == null)
            return;

        ChangeEquipment(armor, m_armor);
        m_armor = armor;
        OnEquipmentChanged?.Invoke();
    }

    public void EquipGear(Equipment gear)
    {
        if (gear == null)
            return;

        ChangeEquipment(gear, m_gear);
        m_gear = gear;
        OnEquipmentChanged?.Invoke();
    }

    public void EquipAccessory(Equipment accessory)
    {
        if (accessory == null)
            return;

        ChangeEquipment(accessory, m_accessorySlot);
        m_accessorySlot = accessory;
        OnEquipmentChanged?.Invoke();
    }

    private void ChangeEquipment(Equipment newEquip, Equipment oldEquip)
    {
        if (oldEquip != null)
        {
            if(oldEquip.MainModifier != null)
                m_equiper.Stats.GetStat(oldEquip.MainModifier.Type).RemoveModifier(oldEquip.MainModifier);

            if(oldEquip.SecondaryModifier != null)
                m_equiper.Stats.GetStat(oldEquip.SecondaryModifier.Type).RemoveModifier(oldEquip.SecondaryModifier);

            oldEquip.Modifiers.ForEach(x => m_equiper.Stats.GetStat(x.Type).RemoveModifier(x));
        }

        if(newEquip.MainModifier != null)
            m_equiper.Stats.GetStat(newEquip.MainModifier.Type).AddModifier(newEquip.MainModifier);

        if(newEquip.SecondaryModifier != null)
            m_equiper.Stats.GetStat(newEquip.SecondaryModifier.Type).AddModifier(newEquip.SecondaryModifier);

        newEquip.Modifiers.ForEach(x => m_equiper.Stats.GetStat(x.Type).AddModifier(x));
    }

    private void HandleArchetypeStatModifierChange(StatModifier[] oldModifiers, StatModifier[] newModifiers)
    {
        oldModifiers?.ForEach(x => m_equiper.Stats.GetStat(x.Type).RemoveModifier(x));
        newModifiers?.ForEach(x => m_equiper.Stats.GetStat(x.Type).AddModifier(x));
    }
}
