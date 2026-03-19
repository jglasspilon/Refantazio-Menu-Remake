using System;
using UnityEngine;
using UnityEngine.Events;

public class ItemSelecter : UIObjectSelecter<InventoryItemUI>
{
    [SerializeField] private UnityEvent<InventoryEntry> OnItemChanged;
    public Action<InventoryEntry> OnItemSelected;

    protected void OnEnable()
    {
        m_parentSection.OnConfirm.AddListener(SelectItem);
    }

    private void OnDisable()
    {
        m_parentSection.OnConfirm.RemoveListener(SelectItem);
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
