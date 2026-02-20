using UnityEngine;

public abstract class InventoryItem : PoolableObject
{
    public abstract InventoryEntry InventoryEntry { get; }
    public abstract void Initialize(InventoryEntry entry);
    public virtual void SetAsSelected(bool selected)
    {
        if (selected)
            InventoryEntry.MarkAsSeen();
    }
}
