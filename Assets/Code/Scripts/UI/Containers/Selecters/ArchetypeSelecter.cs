using System;
using UnityEngine;

public class ArchetypeSelecter : UIObjectSelecter<ArchetypeBanner>
{
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
}
