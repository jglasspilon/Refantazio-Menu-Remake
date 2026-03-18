using System;
using UnityEngine;

public class EquipmentMenuPage : MenuPage
{
    [SerializeField] private CharacterSelecter m_characterSelecter;
    [SerializeField] private EquipmentSlotSelecter m_slotSelecter;
    [SerializeField] private ArchetypeSelecter m_archetypeSelecter;
    [SerializeField] private EquipmentSlotSelectionSection m_equipmentSlotSelectionSection;
    [SerializeField] private ArchetypeSelectionSection m_archetypeSelectionSection;

    private Character m_selectedCharacter;
    private EEquipmentSlotType m_selectedSlotType;

    private void OnEnable()
    {
        m_archetypeSelecter.OnArchetypeSelected += HandleOnArchetypeSelected;
        m_characterSelecter.OnCharacterSelected += HandleOnSelectedCharacter;
        m_slotSelecter.OnSlotSelected += HandleSlotSelected;
    }

    private void OnDisable()
    {
        m_archetypeSelecter.OnArchetypeSelected -= HandleOnArchetypeSelected;
        m_characterSelecter.OnCharacterSelected -= HandleOnSelectedCharacter;
        m_slotSelecter.OnSlotSelected -= HandleSlotSelected;
    }

    private void HandleOnSelectedCharacter(Character character)
    {
        m_selectedCharacter = character;
        EnterSection(m_equipmentSlotSelectionSection);
    }

    private void HandleSlotSelected(EEquipmentSlotType slotType)
    {
        m_selectedSlotType = slotType;

        if (slotType == EEquipmentSlotType.Archetype)
        {
            EnterSection(m_archetypeSelectionSection);
            return;
        }

        else
        {
            Debug.Log("Open equipment selection menu");
            return;
        }
    }

    private void HandleOnArchetypeSelected(Archetype archetype)
    {
        if(m_selectedCharacter != null)
            m_selectedCharacter.Equipment.EquipArchetype(archetype);

        TryExitCurrentSection();
    }
}
