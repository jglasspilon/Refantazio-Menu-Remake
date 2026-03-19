using System;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentSlotSelecter : UIObjectSelecter<SelectableSlot>
{
    [SerializeField] private UnityEvent<EEquipmentSlotType> OnSlotChanged;
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
    }

    protected override void VirtualInvoke()
    {
        OnSlotChanged?.Invoke(SelectedObject == null ? EEquipmentSlotType.Undetermined : SelectedObject.SlotType);
    }
}
