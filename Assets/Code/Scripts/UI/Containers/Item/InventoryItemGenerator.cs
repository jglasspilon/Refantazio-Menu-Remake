using UnityEngine;

public class InventoryItemGenerator: UIObjectGeneraterFromPool<InventoryItem, InventoryEntry>
{
    [SerializeField]
    private InventoryItem m_itemPrefab;

    [SerializeField]
    private InventoryItem m_equipmentPrefab;

    [SerializeField]
    private InventoryItem m_accessoryPrefab;

    protected override void GeneratePoolableFromData(InventoryEntry entry)
    {
        InventoryItem chosenPrefab = GetPrefabFromItemType(entry.Item);
        InventoryItem newItem = m_assetPool.PullFrom(chosenPrefab.GetType(), m_holder) as InventoryItem;
        newItem.InitializeFromData(entry);
        m_content.Add(newItem);
    }   

    private InventoryItem GetPrefabFromItemType(Item item)
    {
        return item switch
        {
            Equipment => m_equipmentPrefab,
            Accessory => (item as Accessory).Effect == null ? m_itemPrefab : m_accessoryPrefab,
            _ => m_itemPrefab
        };
    }
}
