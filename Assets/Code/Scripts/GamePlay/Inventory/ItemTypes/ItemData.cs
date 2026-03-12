using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Items/Non-usable Item")]
public class ItemData : UniqueScriptableObject
{
    [SerializeField] protected string m_name;
    [SerializeField] protected int m_sortOrder;
    [SerializeField] protected EItemCategories m_category;
    [SerializeField] protected Sprite m_icon;
    [SerializeField] protected int m_price;  
    [TextArea]
    [SerializeField] protected string m_description;

    public string Name => m_name;
    public string Description => m_description;
    public EItemCategories Category => m_category;  
    public int Price => m_price;
    public Sprite Icon => m_icon;
    public int SortOrder => m_sortOrder;

    public virtual Item CreateItemFromData()
    {
        Item item = new Item(m_id, m_name, m_sortOrder, m_category, m_icon, m_price, m_description);
        item.InitializeProperties();
        return item;
    }
}

[Serializable]
public class Item: IPropertyProvider
{
    [SerializeField] protected ObservableProperty<string> m_name = new ObservableProperty<string>();
    [SerializeField] protected ObservableProperty<string> m_description = new ObservableProperty<string>();
    [SerializeField] protected ObservableProperty<Sprite> m_icon = new ObservableProperty<Sprite>();
    [SerializeField] protected ObservableProperty<int> m_price = new ObservableProperty<int>();
    [SerializeField] protected EItemCategories m_category;
    [SerializeField] protected int m_sortOrder;

    private string m_id;
    private Dictionary<string, IObservableProperty> m_properties;
    private bool m_isInitialized;

    public string ID => m_id;
    public string Name => m_name.Value;
    public string Description => m_description.Value;
    public EItemCategories Category => m_category;
    public int Price => m_price.Value;
    public Sprite Icon => m_icon.Value;
    public int SortOrder => m_sortOrder;

    public Item(string id, string name, int sortOrder, EItemCategories category, Sprite icon, int price, string description)
    {
        m_id = id;
        m_name.Value = name;
        m_sortOrder = sortOrder;
        m_category = category;
        m_icon.Value = icon;
        m_price.Value = price;
        m_description.Value = description;        
    }

    public void InitializeProperties()
    {
        if (m_isInitialized)
            return;

        m_properties = Helper.DataHandling.BuildPropertyMap(this);
        m_isInitialized = true;
    }

    public bool TryGetPropertyRaw(string key, out object value)
    {
        InitializeProperties();
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
        InitializeProperties();
        if (m_properties.TryGetValue(key, out IObservableProperty raw) && raw is ObservableProperty<T> typed)
        {
            value = typed;
            return true;
        }

        value = null;
        return false;
    }
}

public enum EItemCategories
{
    Usable,
    Valuable,
    KeyItem,
    QuestItem,
    Weapon,
    Armor,
    Gear,
    Accessory,
    All
}