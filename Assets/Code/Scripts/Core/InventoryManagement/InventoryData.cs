using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventoryData
{
    public event Action<InventoryEntry> OnItemAdded;
    public event Action<InventoryEntry> OnItemRemoved;
    
    [SerializeField]
    private List<InventoryEntry> m_orderedEntries = new List<InventoryEntry>();

    [SerializeField]
    private LoggingProfile m_logProfile;

    private Dictionary<string, InventoryEntry> m_entries = new Dictionary<string, InventoryEntry>();
    
    public InventoryEntry[] GetAllItems(EItemCategories category)
    {
        if (category == EItemCategories.All)
            return m_orderedEntries.OrderBy(x => x.Item.Category).ThenBy(x => x.Item.SortOrder).ToArray();

        IEnumerable<InventoryEntry> entries = m_orderedEntries.Where(x => x.Item.Category == category);

        if (category == EItemCategories.Usable)
            return entries.OrderBy(x => (x.Item as UsableItem).BattleOnly == false).ThenBy(x => x.Item.SortOrder).ToArray();

        return entries.OrderBy(x => x.Item.SortOrder).ToArray();
    }

    public InventoryEntry GetItem(string id)
    {
        if(m_entries.TryGetValue(id, out InventoryEntry entry))
            return entry;

        Logger.LogError($"Failed to get Item for ID {id}. No item registered to the inventory with that ID.", m_logProfile);
        return null;
    }

    public void AddOrRemoveItem(Item newItem, int amount)
    {
        if(!m_entries.TryGetValue(newItem.ID, out InventoryEntry entry))
        {
            entry = new InventoryEntry(newItem);
            m_entries.Add(newItem.ID, entry);
            m_orderedEntries.Add(entry);
        }

        entry.ApplyAmount(amount);

        if (amount > 0)
        {
            Logger.Log($"Added {amount} {newItem.Name} to the inventory for a total of {entry.Count}", m_logProfile);
            OnItemAdded?.Invoke(entry);
        }
        else if (amount < 0)
        {
            Logger.Log($"removed {amount} {newItem.Name} from the inventory for a total of {entry.Count}", m_logProfile);
            OnItemRemoved?.Invoke(entry);
        }
    }
}

[Serializable]
public class InventoryEntry
{
    public event Action<int> OnAmountChanged;
    public event Action OnMarkAsSeen;

    [SerializeField]
    private Item m_item;

    [SerializeField]
    private int m_count;

    [SerializeField]
    private bool m_isNew = true;

    public string ID => m_item.ID;
    public Item Item => m_item; 
    public int Count => m_count;
    public bool IsNew => m_isNew;

    public InventoryEntry(Item item)
    {
        m_item = item;
        m_isNew = true;
    }

    public void ApplyAmount(int amount)
    {
        m_count = Mathf.Clamp(m_count + amount, 0, 99);
        OnAmountChanged?.Invoke(amount);
    }

    public void MarkAsSeen()
    {
        m_isNew = false;
        OnMarkAsSeen?.Invoke();
    }
}

