using UnityEngine;

public class EquipmentMenuPage : MenuPage
{
    [SerializeField] private CharacterSelecter m_characterSelecter;
    [SerializeField] private EquipmentSlotSelectionSection m_equipmentSlotSelectionSection;

    private Character m_selectedCharacter;

    private void OnEnable()
    {
        m_characterSelecter.OnCharacterSelected += HandleOnSelectedCharacter;
    }

    private void OnDisable()
    {
        m_characterSelecter.OnCharacterSelected -= HandleOnSelectedCharacter;
    }

    private void HandleOnSelectedCharacter(Character character)
    {
        m_selectedCharacter = character;
        EnterSection(m_equipmentSlotSelectionSection);
    }
}
