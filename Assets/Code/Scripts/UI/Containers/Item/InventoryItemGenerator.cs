using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemGenerator: IDisposable
{
    [SerializeField]
    private InventoryItem m_itemPrefab;

    [SerializeField]
    private InventoryItem m_equipmentPrefab;

    [SerializeField]
    private Transform m_holder;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private List<InventoryItem> m_items = new List<InventoryItem>();
    private InventoryData m_inventoryData;
    private AssetPoolManager m_assetPool;

    public void Dispose()
    {
        ClearInventory();
    }

    public void Initialize(InventoryData inventoryData, AssetPoolManager assetPool)
    {
        m_inventoryData = inventoryData;
        m_assetPool = assetPool;
    }

    public InventoryItem[] GenerateInventory(EItemCategories category)
    {
        if (m_inventoryData == null)
        {
            Logger.LogError($"Could not update inventory. No Inventory data registered.", m_logProfile);
            return Array.Empty<InventoryItem>();
        }

        if (m_assetPool == null)
        {
            Logger.LogError($"Could not update inventory. No asset pool found.", m_logProfile);
            return Array.Empty<InventoryItem>(); ;
        }

        ClearInventory();
        InventoryEntry[] itemsToGenerate = m_inventoryData.GetAllItems(category);
        foreach (InventoryEntry item in itemsToGenerate)
        {
            GenerateItem(item);
        }

        return m_items.ToArray();
    }

    public void ClearInventory()
    {
        if (m_items.Count > 0 && m_assetPool != null)
        {
            m_assetPool.ReturnToPool(m_items);
            m_items.Clear();
        }
    }

    private void GenerateItem(InventoryEntry item)
    {
        InventoryItem chosenPrefab = (item.Item is Equipment) ? m_equipmentPrefab : m_itemPrefab;
        InventoryItem newItem = m_assetPool.PullFrom(chosenPrefab.GetType(), m_holder) as InventoryItem;
        newItem.Initialize(item);
        m_items.Add(newItem);
    }    
}
