using UnityEngine;

public abstract class InventoryItem : PoolableObject, ISelectable
{
    public abstract InventoryEntry InventoryEntry { get; }
    public abstract void Initialize(InventoryEntry entry);
    public abstract void Select();

    public virtual void SetAsSelected(bool selected)
    {
        if (selected)
            InventoryEntry.MarkAsSeen();
    }
}
