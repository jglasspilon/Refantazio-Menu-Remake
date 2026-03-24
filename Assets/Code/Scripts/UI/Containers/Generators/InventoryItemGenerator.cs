using UnityEngine;

public class InventoryItemGenerator: UIObjectGeneraterFromPool<InventoryItemUI, InventoryEntry>
{
    [SerializeField]
    private InventoryItemUI m_itemPrefab;

    [SerializeField]
    private InventoryItemUI m_equipmentPrefab;

    protected override void GeneratePoolableFromData(InventoryEntry entry)
    {
        InventoryItemUI chosenPrefab = GetPrefabFromItemType(entry.Item);
        InventoryItemUI newItem = m_assetPool.PullFrom(chosenPrefab.GetType(), m_holder) as InventoryItemUI;
        newItem.InitializeFromData(entry);
        m_content.Add(newItem);
    }   

    private InventoryItemUI GetPrefabFromItemType(Item item)
    {
        return item switch
        {
            Equipment => m_equipmentPrefab,
            _ => m_itemPrefab
        };
    }
}
