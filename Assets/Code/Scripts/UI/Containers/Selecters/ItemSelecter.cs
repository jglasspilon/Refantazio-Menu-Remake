using System;
using UnityEngine;
using UnityEngine.Events;

public class ItemSelecter : UIObjectSelecter<InventoryItemUI>
{
    [SerializeField] private UnityEvent<InventoryEntry> OnItemChanged;
    public Action<InventoryEntry> OnItemSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += SelectItem;
    }

    public void SelectItem()
    {
        OnItemSelected?.Invoke(SelectedObject.InventoryEntry);
    }

    protected override void VirtualInvoke()
    {
        OnItemChanged?.Invoke(SelectedObject.InventoryEntry);
    }
}
