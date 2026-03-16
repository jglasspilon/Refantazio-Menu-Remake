using System;
using System.Linq;
using UnityEngine;

public abstract class SelectableSlot : MonoBehaviour, ISelectable
{
    [SerializeField] private ESlotType m_slotType;
    [SerializeField] private LoggingProfile m_logProfile;

    private object m_slotContent;
    private IBindableToProperty[] m_bindables;

    public event Action<bool> OnSetAsSelected;
    public event Action<bool> OnSetAsSelectable;

    public object SlotContent => m_slotContent;
    public ESlotType SlotType => m_slotType;

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToProperty>();
    }

    public void InitializeWithProvider(IPropertyProvider provider)
    {
        m_bindables.ForEach(x => x.BindToProperty(provider));
    }

    public void PauseSelection()
    {
        //Does nothing
    }

    public void SetAsSelectable(bool selectable)
    {
        OnSetAsSelectable?.Invoke(selectable);
    }

    public virtual void SetAsSelected(bool selected)
    {
        OnSetAsSelected?.Invoke(selected);
    }

    public ESlotType Select()
    {
        return m_slotType;
    }
}

public enum ESlotType
{
    Archetype,
    Weapon, 
    Armor,
    Gear,
    Accessory,
    Undetermined
}
