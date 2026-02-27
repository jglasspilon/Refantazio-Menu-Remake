using System;
using UnityEngine;

public abstract class InventoryItemUI : PoolableObjectFromData<InventoryEntry>, ISelectable
{
    public event Action<bool> OnSetAsSelected, OnSetAsSelectable;
    public abstract InventoryEntry InventoryEntry { get; }
    public virtual void SetAsSelected(bool selected)
    {
        if (selected)
            InventoryEntry.MarkAsSeen();

        OnSetAsSelected?.Invoke(selected);
    }
    public virtual void SetAsSelectable(bool selectable)
    {
        OnSetAsSelectable?.Invoke(selectable);
    }
    public abstract void PauseSelection();
    
}
