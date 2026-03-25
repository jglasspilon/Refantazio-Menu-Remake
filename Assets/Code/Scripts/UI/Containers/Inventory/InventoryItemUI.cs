using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class InventoryItemUI : PoolableObjectFromData<InventoryEntry>, ISelectable
{
    public event Action<bool> OnSetAsSelected, OnSetAsSelectable;

    [SerializeField]
    protected LoggingProfile m_logProfile;

    protected InventoryEntry m_inventoryEntry;
    private IBindableToProperty[] m_bindablesToEntry;
    private IBindableToProperty[] m_bindablesToItem;
    private IBindableToProperty[] m_bindablesToEquipment;
    private IBindableToInventoryEntry[] m_bindableToItems;
    public InventoryEntry InventoryEntry => m_inventoryEntry;

    protected virtual void Awake()
    {
        IBindableToProperty[] allProperties = GetComponentsInChildren<IBindableToProperty>();
        List<IBindableToProperty> entryList = new List<IBindableToProperty>();
        List<IBindableToProperty> itemList = new List<IBindableToProperty>();
        List<IBindableToProperty> equipmentList = new List<IBindableToProperty>();

        foreach (var b in allProperties)
        {
            var t = b.ProviderType;

            if (t == typeof(InventoryEntry)) 
                entryList.Add(b);
            else if (t == typeof(Item)) 
                itemList.Add(b);
            else if (t == typeof(Equipment)) 
                equipmentList.Add(b);
        }

        m_bindablesToEntry = entryList.ToArray();
        m_bindablesToItem = itemList.ToArray();
        m_bindablesToEquipment = equipmentList.ToArray();

        m_bindableToItems = GetComponentsInChildren<IBindableToInventoryEntry>();
    }

    public override void InitializeFromData(InventoryEntry entry)
    {
        if (entry == null || entry.Item == null)
        {
            Logger.LogError("Received an empty item. Initializing an empty item is not allowed.", m_logProfile);
            return;
        }

        transform.localScale = Vector3.one;
        m_inventoryEntry = entry;
        SetAsSelectable(entry.Item is not UsableItem usable || !usable.BattleOnly);
        SetAsSelected(false);

        m_bindablesToEntry.ForEach(x => x.BindToProperty(entry));
        m_bindablesToItem.ForEach(x => x.BindToProperty(entry.Item));

        if (entry.Item is Equipment equip)
            m_bindablesToEquipment.ForEach(x => x.BindToProperty(equip));

        m_bindableToItems.ForEach(x => x.BindToInventoryEntry(entry));
    }

    public override void ResetForPool()
    {
        SetAsSelected(false);
        m_inventoryEntry = null;
        m_bindablesToEntry.ForEach(x => x.UnBind());
        m_bindablesToItem.ForEach(x => x.UnBind()); 
        m_bindablesToEquipment.ForEach(x => x.UnBind());

        m_bindableToItems.ForEach(x => x.Unbind());
    }

    public virtual void SetAsSelected(bool selected)
    {
        if (selected)
            InventoryEntry.MarkAsSeen();

        OnSetAsSelected?.Invoke(selected);
    }
    public virtual void SetAsSelectable(bool selectable)
    {
        OnSetAsSelectable?.Invoke(selectable);
    }

    public abstract void PauseSelection();   
}
