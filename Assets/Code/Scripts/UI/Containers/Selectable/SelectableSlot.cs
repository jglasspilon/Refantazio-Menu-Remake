using System;
using UnityEngine;

public class SelectableSlot : MonoBehaviour, ISelectable
{
    [SerializeField] private EEquipmentSlotType m_slotType;
    [SerializeField] private LoggingProfile m_logProfile;

    private object m_slotContent;
    private IBindableToProperty[] m_bindables;
    private Character m_character;

    public event Action<bool> OnSetAsSelected;
    public event Action<bool> OnSetAsSelectable;

    public object SlotContent => m_slotContent;
    public EEquipmentSlotType SlotType => m_slotType;

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToProperty>();
    }

    public void ResetBinding()
    {
        if(m_character != null)
        {
            m_character.Equipment.OnEquipmentChanged -= HandleOnEquipmentChange;
            m_character = null;
        }

        m_slotContent = null;
        m_bindables.ForEach(x => x.UnBind());
    }

    public void InitializeEquipedCharacter(Character character)
    {
        if (character == m_character)
            return;

        if (m_character != null)
            m_character.Equipment.OnEquipmentChanged -= HandleOnEquipmentChange;

        m_character = character;
        m_character.Equipment.OnEquipmentChanged += HandleOnEquipmentChange;
    }

    public void InitializeWithProvider(IPropertyProvider provider)
    {
        if (provider == null)
            return;

        m_slotContent = provider;
        m_bindables.ForEach(x => x.BindToProperty(provider));        
    }

    public void PauseSelection()
    {
        //Does nothing
    }

    public void SetAsSelectable(bool selectable)
    {
        OnSetAsSelectable?.Invoke(selectable);
    }

    public virtual void SetAsSelected(bool selected)
    {
        OnSetAsSelected?.Invoke(selected);
    }

    public EEquipmentSlotType Select()
    {
        return m_slotType;
    }

    private void HandleOnEquipmentChange()
    {
        if (m_character == null)
            return;

        IPropertyProvider newProvider = null;

        switch (m_slotType)
        {
            case EEquipmentSlotType.Archetype:
                newProvider = m_character.Equipment.Archetype;
                break;
            case EEquipmentSlotType.Weapon:
                newProvider = m_character.Equipment.Weapon;
                break;
            case EEquipmentSlotType.Armor:
                newProvider = m_character.Equipment.Armor;
                break;
            case EEquipmentSlotType.Gear:
                newProvider = m_character.Equipment.Gear;
                break;
            case EEquipmentSlotType.Accessory:
                newProvider = m_character.Equipment.Weapon;
                break;
        }

        InitializeWithProvider(newProvider);
    }
}

public enum EEquipmentSlotType
{
    Archetype,
    Weapon, 
    Armor,
    Gear,
    Accessory,
    Undetermined
}
