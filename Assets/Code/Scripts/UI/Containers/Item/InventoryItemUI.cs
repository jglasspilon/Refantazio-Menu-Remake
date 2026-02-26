using UnityEngine;

public abstract class InventoryItemUI : PoolableObjectFromData<InventoryEntry>, ISelectable
{
    public abstract InventoryEntry InventoryEntry { get; }
    public virtual void SetAsSelected(bool selected)
    {
        if (selected)
            InventoryEntry.MarkAsSeen();
    }
    public abstract void SetAsSelectable(bool selectable);
    public abstract void PauseSelection();
    
}
