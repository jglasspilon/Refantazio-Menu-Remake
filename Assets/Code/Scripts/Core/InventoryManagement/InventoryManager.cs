using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryData m_inventoryData;
    [SerializeField] private InventoryEntryData[] m_startingInventory;
    [SerializeField] private int m_startingMoney, m_startingMag;

    void Awake()
    {
        ObjectResolver.Instance.Register(m_inventoryData);
        InitializeStartingInventory();
    }

    private void InitializeStartingInventory()
    {
        foreach (InventoryEntryData entry in m_startingInventory)
        {
            m_inventoryData.AddOrRemoveItem(entry.Item, entry.Count);
        }

        m_inventoryData.Money.Apply(m_startingMoney);
        m_inventoryData.Magla.Apply(m_startingMag);
    }
}
