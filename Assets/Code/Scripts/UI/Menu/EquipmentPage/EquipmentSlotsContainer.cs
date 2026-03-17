using System.Linq;
using UnityEngine;

public class EquipmentSlotsContainer : MonoBehaviour
{
    [SerializeField] private CharacterSelecter m_characterSelecter;
    [SerializeField] private EquipmentSlotSelecter m_slotSelecter;

    private SelectableSlot[] m_slots;
    private Character m_registeredCharacter;

    private void Awake()
    {
        m_slots = GetComponentsInChildren<SelectableSlot>();
    }

    private void OnEnable()
    {
        m_slots.ForEach(x => x.SetAsSelected(false));
        m_characterSelecter.OnSelectedObjectChanged += ProvideCharacterDataToSlots;
    }

    private void OnDisable()
    {
        m_slots.ForEach(x => x.ResetBinding());
        m_characterSelecter.OnSelectedObjectChanged -= ProvideCharacterDataToSlots;
        m_registeredCharacter = null;
    }

    private void ProvideCharacterDataToSlots(CharacterBanner characterBanner)
    {
        if (characterBanner == null || characterBanner.Character == null)
            return;

        Character character = characterBanner.Character;

        m_registeredCharacter = character;
        m_slotSelecter.UnselectAll();
        m_slotSelecter.ResetSelecter();

        if (character.CharacterType.Value != ECharacterType.Guide)
            m_slotSelecter.UpdateObjects(m_slots);
        else
            m_slotSelecter.UpdateObjects(m_slots.Where(s => s.SlotType != ESlotType.Archetype).ToArray());

        m_slots.ForEach(s => InitializeSlotWithProviderAndCharacter(s, m_registeredCharacter));       
        m_slotSelecter.SelectCurrent();
    }

    private void InitializeSlotWithProviderAndCharacter(SelectableSlot slot, Character character)
    {
        slot.InitializeEquipedCharacter(character);
        switch(slot.SlotType)
        {
            case ESlotType.Archetype:
                slot.gameObject.SetActive(character.CharacterType.Value != ECharacterType.Guide);
                slot.InitializeWithProvider(character.Equipment.Archetype);
                break;
            case ESlotType.Weapon:
                slot.InitializeWithProvider(character.Equipment.Weapon);
                break;
            case ESlotType.Armor:
                slot.InitializeWithProvider(character.Equipment.Armor);
                break;
            case ESlotType.Gear:
                slot.InitializeWithProvider(character.Equipment.Gear);
                break;
            case ESlotType.Accessory:
                slot.InitializeWithProvider(character.Equipment.Accessory);
                break;
        }
    }
}
