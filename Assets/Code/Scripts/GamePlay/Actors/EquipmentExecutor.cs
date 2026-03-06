using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentExecutor
{
    [SerializeField]
    private Archetype m_archetypeSlot;

    [SerializeField]
    private Equipment m_weaponSlot, m_armorSlot, m_gearSlot;

    [SerializeField]
    private Accessory m_accessorySlot;

    private Character m_equiper;

    public Archetype Archetype => m_archetypeSlot;
    public Equipment Weapon => m_weaponSlot;
    public Equipment Armor => m_armorSlot;
    public Equipment Gear => m_gearSlot;
    public Accessory Accessory => m_accessorySlot;

    public EquipmentExecutor(EquipmentExecutor storedEquipment, Archetype startingArchetype, Character equiper)
    {
        m_equiper = equiper;
        EquipArchetype(startingArchetype);
        EquipWeapon(storedEquipment.Weapon);
        EquipArmor(storedEquipment.Armor);
        EquipGear(storedEquipment.Gear);
        EquipAccessory(storedEquipment.Accessory);
    }

    public void EquipArchetype(Archetype archetype)
    {
        if (archetype == null)
            return;
        if (m_archetypeSlot != null)
        {
            m_archetypeSlot.StatModifiers.ForEach(x => m_equiper.Stats.GetStat(x.Type).RemoveModifier(x));
            m_archetypeSlot.OnStatModifiersChanged -= HandleArchetypeStatModifierChange;
        }

        m_archetypeSlot = archetype;
        m_archetypeSlot.StatModifiers.ForEach(x => m_equiper.Stats.GetStat(x.Type).AddModifier(x));
        m_archetypeSlot.OnStatModifiersChanged += HandleArchetypeStatModifierChange;
        EquipWeapon(m_archetypeSlot.DefaultWeapon);
    }

    public void EquipWeapon(Equipment weapon)
    {
        if (weapon == null)
            return;

        if (m_archetypeSlot != null && m_archetypeSlot.EquipableWeaponType != weapon.EquipType)
            return;

        ChangeEquipment(weapon, m_weaponSlot);
        m_weaponSlot = weapon;
    }

    public void EquipArmor(Equipment armor)
    {
        if (armor == null)
            return;

        ChangeEquipment(armor, m_armorSlot);
        m_armorSlot = armor;
    }

    public void EquipGear(Equipment gear)
    {
        if (gear == null)
            return;

        ChangeEquipment(gear, m_gearSlot);
        m_gearSlot = gear;
    }

    public void EquipAccessory(Accessory accessory)
    {
        if (accessory == null)
            return;

        ChangeAccessory(accessory, m_accessorySlot);
        m_accessorySlot = accessory;
    }

    private void ChangeEquipment(Equipment newEquip, Equipment oldEquip)
    {
        if (oldEquip != null)
        {
            m_equiper.Stats.GetStat(oldEquip.MainModifier.Type).RemoveModifier(oldEquip.MainModifier);
            m_equiper.Stats.GetStat(oldEquip.SecondaryModifier.Type).RemoveModifier(oldEquip.SecondaryModifier);
            oldEquip.Modifiers.ForEach(x => m_equiper.Stats.GetStat(x.Type).RemoveModifier(x));
        }

        m_equiper.Stats.GetStat(newEquip.MainModifier.Type).AddModifier(newEquip.MainModifier);
        m_equiper.Stats.GetStat(newEquip.SecondaryModifier.Type).AddModifier(newEquip.SecondaryModifier);
        newEquip.Modifiers.ForEach(x => m_equiper.Stats.GetStat(x.Type).AddModifier(x));
    }

    private void ChangeAccessory(Accessory newAccessory, Accessory oldAccessory)
    {
        if (oldAccessory != null)
        {
            oldAccessory.Modifiers.ForEach(x => m_equiper.Stats.GetStat(x.Type).RemoveModifier(x));
        }

        newAccessory.Modifiers.ForEach(x => m_equiper.Stats.GetStat(x.Type).AddModifier(x));
    }

    private void HandleArchetypeStatModifierChange(StatModifier[] oldModifiers, StatModifier[] newModifiers)
    {
        oldModifiers?.ForEach(x => m_equiper.Stats.GetStat(x.Type).RemoveModifier(x));
        newModifiers?.ForEach(x => m_equiper.Stats.GetStat(x.Type).AddModifier(x));
    }
}
