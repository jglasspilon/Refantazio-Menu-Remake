using UnityEngine;

public abstract class MenuItem : PoolableObject
{
    public abstract void Initialize(InventoryEntry entry);
    public abstract void SetAsSelected(bool selected);
}
