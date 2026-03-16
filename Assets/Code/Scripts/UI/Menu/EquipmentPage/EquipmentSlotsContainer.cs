using UnityEngine;

public class EquipmentSlotsContainer : MonoBehaviour
{
    [SerializeField] private CharacterSelecter m_characterSelecter;
    [SerializeField] private EquipmentSlotSelecter m_slotSelecter;
    [SerializeField] private ArchetypeSlot m_archetypeSlot;
    [SerializeField] private EquipmentSlot m_weaponSlot;
    [SerializeField] private EquipmentSlot m_armorSlot;
    [SerializeField] private EquipmentSlot m_gearSlot;
    [SerializeField] private EquipmentSlot m_accessorySlot;

    private Character m_registeredCharacter;

    private void OnEnable()
    {
        foreach (ISelectable selectable in GetComponentsInChildren<ISelectable>())
            selectable.SetAsSelected(false);

        m_characterSelecter.OnSelectedObjectChanged += ProvideCharacterDataToSlots;
    }

    private void ProvideCharacterDataToSlots(CharacterBanner characterBanner)
    {
        if (characterBanner == null || characterBanner.Character == null)
            return;

        Character character = characterBanner.Character;

        if (character == m_registeredCharacter)
            return;

        m_slotSelecter.UnselectAll();
        m_slotSelecter.ResetSelecter();
        m_archetypeSlot.gameObject.SetActive(character.CharacterType.Value != ECharacterType.Guide);

        if (character.CharacterType.Value != ECharacterType.Guide)
        {
            m_slotSelecter.UpdateObjects(m_archetypeSlot, m_weaponSlot, m_armorSlot, m_gearSlot, m_accessorySlot);
            m_archetypeSlot.InitializeWithProvider(character);
        }
        else
            m_slotSelecter.UpdateObjects(m_weaponSlot, m_armorSlot, m_gearSlot, m_accessorySlot);

        m_weaponSlot.InitializeWithProvider(character.Equipment.Weapon);
        m_armorSlot.InitializeWithProvider(character.Equipment.Armor);
        m_gearSlot.InitializeWithProvider(character.Equipment.Gear);
        m_accessorySlot.InitializeWithProvider(character.Equipment.Accessory);
        m_slotSelecter.SelectCurrent();
    }
}
