using System.Collections.Generic;
using UnityEngine;

public class MenuItemGenerator : MonoBehaviour
{
    [SerializeField]
    private MenuItem m_itemPrefab;

    [SerializeField]
    private MenuItem m_equipmentPrefab;

    [SerializeField]
    private Transform m_holder;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private List<MenuItem> m_items = new List<MenuItem>();
    private InventoryData m_inventoryData;
    private AssetPoolManager m_assetPool;
    private EItemCategories m_currentCategoryFilter = EItemCategories.All;
    private int m_currentItemIndexSelected = 0;

    public void OnEnable()
    {
        bool inventoryDataResolved = m_inventoryData != null;
        bool assetPoolResolved = m_assetPool != null;

        if (m_inventoryData == null)
        {
            if (ObjectResolver.Instance.TryResolve(OnInventoryDataChanged, out InventoryData inventoryData))
            {
                OnInventoryDataChanged(inventoryData);
                inventoryDataResolved = true;
            }
        }

        if (m_assetPool == null)
        {
            if (ObjectResolver.Instance.TryResolve(OnAssetPoolChanged, out AssetPoolManager assetPool))
            {
                OnAssetPoolChanged(assetPool);
                assetPoolResolved = true;
            }
        }

        if (!inventoryDataResolved || !assetPoolResolved)
        {
            return;
        }

        GenerateInventory(m_currentCategoryFilter);
    }

    private void OnDisable()
    {
        ClearItems();
        m_currentItemIndexSelected = 0;
    }

    private void OnInventoryDataChanged(InventoryData newReference)
    {
        m_inventoryData = newReference;
        GenerateInventory(m_currentCategoryFilter);
    }

    private void OnAssetPoolChanged(AssetPoolManager assetPool)
    {
        m_assetPool = assetPool;
    }

    private void GenerateInventory(EItemCategories category)
    {
        if (m_inventoryData == null)
        {
            Logger.LogError($"Could not update inventory. No Inventory data registered.", m_logProfile);
            return;
        }

        if (m_assetPool == null)
        {
            Logger.LogError($"Could not update inventory. No asset pool found.", m_logProfile);
            return;
        }

        ClearItems();
        InventoryEntry[] itemsToGenerate = m_inventoryData.GetAllItems(category);
        foreach (InventoryEntry item in itemsToGenerate)
        {
            GenerateItem(item);
        }

        if (m_items.Count == 0)
            return;

        if (m_currentItemIndexSelected > m_items.Count)
            m_currentItemIndexSelected = 0;

        m_items[m_currentItemIndexSelected].SetAsSelected(true);
    }

    private void GenerateItem(InventoryEntry item)
    {
        MenuItem chosenPrefab = (item.Item is Equipment) ? m_equipmentPrefab : m_itemPrefab;
        MenuItem newItem = m_assetPool.PullFrom(chosenPrefab.GetType(), m_holder) as MenuItem;
        newItem.Initialize(item);
        m_items.Add(newItem);
    }

    private void ClearItems()
    {
        if (m_items.Count > 0 && m_assetPool != null)
        {
            m_assetPool.ReturnToPool(m_items);
            m_items.Clear();
        }
    }
}
