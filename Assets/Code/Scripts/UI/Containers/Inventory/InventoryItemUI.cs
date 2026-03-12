using System;
using UnityEngine;

public abstract class InventoryItemUI : PoolableObjectFromData<InventoryEntry>, ISelectable
{
    public event Action<bool> OnSetAsSelected, OnSetAsSelectable;

    [SerializeField]
    protected LoggingProfile m_logProfile;

    protected InventoryEntry m_inventoryEntry;
    private IBindableToProperty[] m_bindables;
    private IBindableToInventoryEntry[] m_bindableToItems;
    public InventoryEntry InventoryEntry => m_inventoryEntry;

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToProperty>();
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

        m_bindables.ForEach(x => x.BindToProperty(entry));
        m_bindableToItems.ForEach(x => x.BindToInventoryEntry(entry));
    }

    public override void ResetForPool()
    {
        SetAsSelected(false);
        m_inventoryEntry = null;
        m_bindables.ForEach(x => x.UnBind());
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
