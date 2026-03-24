using System;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentSlotSelecter : UIObjectSelecter<SelectableSlot>
{
    [SerializeField] private UnityEvent<EEquipmentSlotType> OnSlotChanged;
    [SerializeField] private UnityEvent<Archetype> OnArchetypeSlotSelected;
    [SerializeField] private UnityEvent<Equipment> OnEquipmentSlotSelected;
    public event Action<EEquipmentSlotType> OnSlotSelected;

    protected void OnEnable()
    {
        m_parentSection.OnConfirm.AddListener(SelectSlot);
    }

    private void OnDisable()
    {
        m_parentSection.OnConfirm.RemoveListener(SelectSlot);
    }

    public void SelectSlot()
    {
        OnSlotSelected?.Invoke(SelectedObject == null ? EEquipmentSlotType.Undetermined : SelectedObject.SlotType);

        if(SelectedObject.SlotContent is Archetype archetype)
            OnArchetypeSlotSelected?.Invoke(archetype);

        if(SelectedObject.SlotContent is Equipment equipment)
            OnEquipmentSlotSelected?.Invoke(equipment);
    }

    protected override void VirtualInvoke()
    {
        OnSlotChanged?.Invoke(SelectedObject == null ? EEquipmentSlotType.Undetermined : SelectedObject.SlotType);
    }
}
