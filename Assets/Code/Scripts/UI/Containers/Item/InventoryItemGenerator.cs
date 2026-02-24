using UnityEngine;

public class InventoryItemGenerator: UIObjectGeneraterFromPool<InventoryItem, InventoryEntry>
{
    [SerializeField]
    private InventoryItem m_itemPrefab;

    [SerializeField]
    private InventoryItem m_weaponPrefab;

    [SerializeField]
    private InventoryItem m_armorPrefab;

    [SerializeField]
    private InventoryItem m_igniterPrefab;

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
            Weapon => m_weaponPrefab,
            Armor => m_armorPrefab, 
            Igniter => m_igniterPrefab,
            _ => m_itemPrefab
        };
    }
}
