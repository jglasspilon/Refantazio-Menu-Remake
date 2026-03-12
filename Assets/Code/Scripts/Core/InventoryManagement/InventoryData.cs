using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventoryData
{   
    [SerializeField] private List<InventoryEntry> m_orderedEntries = new List<InventoryEntry>();
    [SerializeField] private Resource m_money = new Resource(9999999);
    [SerializeField] private Resource m_magla = new Resource(9999999);
    [SerializeField] private LoggingProfile m_logProfile;

    public event Action<InventoryEntry> OnItemAdded;
    public event Action<InventoryEntry> OnItemRemoved;
    public event Action<EItemCategories> OnLastMarkUnseen;

    private Dictionary<string, InventoryEntry> m_entries = new Dictionary<string, InventoryEntry>();
    public Resource Money => m_money;
    public Resource Magla => m_magla;

    public InventoryEntry[] GetAllItems(EItemCategories category)
    {
        if (category == EItemCategories.All)
            return m_orderedEntries.Where(x => x.Count > 0).OrderBy(x => x.Item.Category).ThenBy(x => x.Item.SortOrder).ToArray();

        IEnumerable<InventoryEntry> entries = m_orderedEntries.Where(x => x.Item.Category == category && x.Count > 0);

        if (category == EItemCategories.Usable)
            return entries.OrderBy(x => (x.Item as UsableItem).BattleOnly == true).ThenBy(x => x.Item.SortOrder).ToArray();

        return entries.OrderBy(x => x.Item.SortOrder).ToArray();
    }

    public InventoryEntry GetItem(string id)
    {
        if(m_entries.TryGetValue(id, out InventoryEntry entry))
            return entry;

        Logger.LogError($"Failed to get Item for ID {id}. No item registered to the inventory with that ID.", m_logProfile);
        return null;
    }

    public void AddOrRemoveItem(ItemData newItem, int amount)
    {
        if(!m_entries.TryGetValue(newItem.ID, out InventoryEntry entry))
        {
            entry = new InventoryEntry(newItem);
            m_entries.Add(newItem.ID, entry);
            m_orderedEntries.Add(entry);
            entry.OnMarkAsSeen += HandleItemMarkAsSeen;
        }

        entry.ApplyAmount(amount);

        if (amount > 0)
        {
            Logger.Log($"Added {amount} {newItem.Name} to the inventory for a total of {entry.Count}", m_logProfile);
            OnItemAdded?.Invoke(entry);
        }
        else if (amount < 0)
        {
            Logger.Log($"removed {Mathf.Abs(amount)} {newItem.Name} from the inventory for a total of {entry.Count}", m_logProfile);
            OnItemRemoved?.Invoke(entry);
        }
    }

    private void HandleItemMarkAsSeen(InventoryEntry entry)
    {
        EItemCategories category = entry.Item.Category;

        if(GetAllItems(category).Where(x => x.IsNew).Count() == 0)
        {
            OnLastMarkUnseen?.Invoke(category);
        }
    }
}

[Serializable]
public class InventoryEntry: IPropertyProvider
{
    [SerializeField] private Item m_item;
    [SerializeField] private ObservableProperty<int> m_count = new ObservableProperty<int>();
    [SerializeField] private ObservableProperty<bool> m_isNew = new ObservableProperty<bool>();

    private Dictionary<string, IObservableProperty> m_properties;

    public event Action<InventoryEntry> OnMarkAsSeen;

    public string Name => m_item.Name;
    public string ID => m_item.ID;
    public Item Item => m_item; 
    public int Count => m_count.Value;
    public bool IsNew => m_isNew.Value;

    public InventoryEntry(ItemData itemData)
    {
        m_item = itemData.CreateItemFromData();
        m_isNew.Value = true;
        InitializeProperties();
    }

    private void InitializeProperties()
    {
        m_properties = Helper.DataHandling.BuildPropertyMap(this);
    }

    public bool TryGetPropertyRaw(string key, out object value)
    {
        if (m_properties.TryGetValue(key, out IObservableProperty raw))
        {
            value = raw;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetProperty<T>(string key, out ObservableProperty<T> value)
    {
        if (m_properties.TryGetValue(key, out IObservableProperty raw) && raw is ObservableProperty<T> typed)
        {
            value = typed;
            return true;
        }

        value = null;
        return false;
    }

    public void ApplyAmount(int amount)
    {
        m_count.Value = Mathf.Clamp(m_count.Value + amount, 0, 99);
    }

    public void MarkAsSeen()
    {
        m_isNew.Value = false;
        OnMarkAsSeen?.Invoke(this);
    }
}

[Serializable]
public class InventoryEntryData
{
    [SerializeField] private ItemData m_item;
    [SerializeField] private int m_count;

    public string ID => m_item.ID;
    public ItemData Item => m_item;
    public int Count => m_count;

    public InventoryEntry CreateInventoryEntryFromData()
    {
        return new InventoryEntry(m_item);
    }
}

public enum ECurrencyType
{
    Money, 
    Magla
}

