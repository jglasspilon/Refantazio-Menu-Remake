using System;
using UnityEngine;
using UnityEngine.Events;

public class ArchetypeSelecter : UIObjectSelecter<ArchetypeBanner>
{
    [SerializeField] private UnityEvent<Archetype> OnArchetypeChanged;
    public event Action<Archetype> OnArchetypeSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += SelectArchetype;
    }

    private void SelectArchetype()
    {
        OnArchetypeSelected?.Invoke(SelectedObject == null ? null : SelectedObject.Archetype);
    }

    protected override void VirtualInvoke()
    {
        OnArchetypeChanged?.Invoke(SelectedObject == null ? null : SelectedObject.Archetype);
    }
}
