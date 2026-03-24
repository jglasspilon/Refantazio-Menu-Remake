using System;
using UnityEngine;

public class EquipmentMenuPage : MenuPage
{
    [SerializeField] private CharacterSelecter m_characterSelecter;
    [SerializeField] private EquipmentSlotSelecter m_slotSelecter;
    [SerializeField] private ArchetypeSelecter m_archetypeSelecter;
    [SerializeField] private ItemSelecter m_equipmentSelecter;
    [SerializeField] private EquipmentSlotSelectionSection m_equipmentSlotSelectionSection;
    [SerializeField] private ArchetypeSelectionSection m_archetypeSelectionSection;
    [SerializeField] private EquipmentSelectionSection m_equipmentSelectionSection;

    private EEquipmentSlotType m_selectedSlotType;

    private void OnEnable()
    {
        m_archetypeSelecter.OnArchetypeSelected += HandleOnArchetypeSelected;
        m_characterSelecter.OnCharacterSelected += HandleOnSelectedCharacter;
        m_slotSelecter.OnSlotSelected += HandleSlotSelected;
        m_equipmentSelecter.OnItemSelected += HandleEquipmentSelected;
    }

    private void OnDisable()
    {
        m_archetypeSelecter.OnArchetypeSelected -= HandleOnArchetypeSelected;
        m_characterSelecter.OnCharacterSelected -= HandleOnSelectedCharacter;
        m_slotSelecter.OnSlotSelected -= HandleSlotSelected;
        m_equipmentSelecter.OnItemSelected -= HandleEquipmentSelected;
    }

    private void HandleOnSelectedCharacter(Character character)
    {
        EnterSection(m_equipmentSlotSelectionSection);
    }

    private void HandleSlotSelected(EEquipmentSlotType slotType)
    {
        if (m_characterSelecter.SelectedObject.Character.IsGuide)
            return;

        m_selectedSlotType = slotType;

        if (slotType == EEquipmentSlotType.Archetype)
        {
            EnterSection(m_archetypeSelectionSection);
            return;
        }

        else
        {
            EnterSection(m_equipmentSelectionSection);
            return;
        }
    }

    private void HandleOnArchetypeSelected(Archetype archetype)
    {
        m_characterSelecter.SelectedObject.Character.Equipment.EquipArchetype(archetype);
        TryExitCurrentSection();
    }

    private void HandleEquipmentSelected(InventoryEntry entry)
    {
        if (entry.Item is not Equipment equipment)
            return;

        Character selectedCharacter = m_characterSelecter.SelectedObject.Character;
        switch(m_selectedSlotType)
        {
            case EEquipmentSlotType.Weapon:
                selectedCharacter.Equipment.EquipWeapon(equipment);
                break;
            case EEquipmentSlotType.Armor:
                selectedCharacter.Equipment.EquipArmor(equipment);
                break;
            case EEquipmentSlotType.Gear:
                selectedCharacter.Equipment.EquipGear(equipment);
                break;
            case EEquipmentSlotType.Accessory:
                selectedCharacter.Equipment.EquipAccessory(equipment);
                break;
        }

        TryExitCurrentSection();
    }
}
