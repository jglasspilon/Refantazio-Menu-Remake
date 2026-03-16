using System;
using UnityEngine;

public class EquipmentSlotSelecter : UIObjectSelecter<SelectableSlot>
{
    public event Action<ESlotType, object> OnSlotSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += SelectSlot;
    }

    public void SelectSlot()
    {
        OnSlotSelected?.Invoke(SelectedObject == null ? ESlotType.Undetermined : SelectedObject.SlotType, SelectedObject == null ? null : SelectedObject.SlotContent);
    }
}
