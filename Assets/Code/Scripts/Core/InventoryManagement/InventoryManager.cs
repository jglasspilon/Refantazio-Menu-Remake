using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private InventoryData m_inventoryData;

    [SerializeField]
    private InventoryEntry[] m_startingInventory;

    void Awake()
    {
        ObjectResolver.Instance.Register(m_inventoryData);
        InitializeStartingInventory();
    }

    private void InitializeStartingInventory()
    {
        foreach (InventoryEntry entry in m_startingInventory)
        {
            m_inventoryData.AddOrRemoveItem(entry.Item, entry.Count);
        }
    }
}
