using System;
using UnityEngine;

public class EquipmentMenuPage : MenuPage
{
    [SerializeField] private CharacterSelecter m_characterSelecter;
    [SerializeField] private EquipmentSlotSelecter m_slotSelecter;
    [SerializeField] private EquipmentSlotSelectionSection m_equipmentSlotSelectionSection;

    private Character m_selectedCharacter;
    private ESlotType m_selectedSlotType;

    private void OnEnable()
    {
        m_characterSelecter.OnCharacterSelected += HandleOnSelectedCharacter;
        m_slotSelecter.OnSlotSelected += HandleSlotSelected;
    }

    private void OnDisable()
    {
        m_characterSelecter.OnCharacterSelected -= HandleOnSelectedCharacter;
        m_slotSelecter.OnSlotSelected -= HandleSlotSelected;
    }

    private void HandleOnSelectedCharacter(Character character)
    {
        m_selectedCharacter = character;
        EnterSection(m_equipmentSlotSelectionSection);
    }

    private void HandleSlotSelected(ESlotType slotType, object obj)
    {
        m_selectedSlotType = slotType;

        if (slotType == ESlotType.Archetype)
        {
            Debug.Log("Open archetype selection menu");
            return;
        }

        else
        {
            Debug.Log("Open equipment selection menu");
            return;
        }
    }
}
