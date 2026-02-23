using System;
using UnityEngine;

[Serializable]
public class InventoryItemGenerator: UIObjectGeneraterFromPool<InventoryItem, InventoryEntry>
{
    [SerializeField]
    private InventoryItem m_itemPrefab;

    [SerializeField]
    private InventoryItem m_equipmentPrefab;

    protected override void GeneratePoolableFromData(InventoryEntry data)
    {
        InventoryItem chosenPrefab = (data.Item is Equipment) ? m_equipmentPrefab : m_itemPrefab;
        InventoryItem newItem = m_assetPool.PullFrom(chosenPrefab.GetType(), m_holder) as InventoryItem;
        newItem.InitializeFromData(data);
        m_content.Add(newItem);
    }   
}
