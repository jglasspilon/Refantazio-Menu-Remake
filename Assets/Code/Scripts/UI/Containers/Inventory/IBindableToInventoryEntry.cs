using UnityEngine;

public interface IBindableToInventoryEntry
{
    public void BindToInventoryEntry(InventoryEntry entry);
    public void Unbind();
}
