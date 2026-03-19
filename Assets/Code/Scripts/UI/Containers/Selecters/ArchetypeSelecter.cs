using System;
using UnityEngine;
using UnityEngine.Events;

public class ArchetypeSelecter : UIObjectSelecter<ArchetypeBanner>
{
    [SerializeField] private UnityEvent<Archetype> OnArchetypeChanged;
    public event Action<Archetype> OnArchetypeSelected;

    protected void OnEnable()
    {
        m_parentSection.OnConfirm.AddListener(SelectArchetype);
    }

    private void OnDisable()
    {
        m_parentSection.OnConfirm.RemoveListener(SelectArchetype);
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
