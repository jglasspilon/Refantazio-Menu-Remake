using System;
using UnityEngine;

public class ArchetypeSelecter : UIObjectSelecter<ArchetypeSlot>
{
    public event Action<Archetype> OnSlotSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += SelectSlot;
    }

    public void SelectSlot()
    {
        OnSlotSelected?.Invoke(SelectedObject == null ? null : SelectedObject.Archetype);
    }
}
