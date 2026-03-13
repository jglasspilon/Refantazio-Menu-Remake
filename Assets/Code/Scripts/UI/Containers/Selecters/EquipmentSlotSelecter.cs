using System;
using UnityEngine;

public class EquipmentSlotSelecter : UIObjectSelecter<EquipmentSlot>
{
    public event Action<Equipment> OnCharacterSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += SelectSlot;
    }

    public void SelectSlot()
    {
        OnCharacterSelected?.Invoke(SelectedObject == null ? null : SelectedObject.Equipment);
    }
}
