using System;
using UnityEngine;

public class ArchetypeBanner : PoolableObjectFromData<Archetype>, ISelectable
{
    [SerializeField] private GameObject m_isEquipedContent;

    private Archetype m_archetype;
    private IBindableToProperty[] m_bindables;

    public event Action<bool> OnSetAsSelected;
    public event Action<bool> OnSetAsSelectable;

    public Archetype Archetype => m_archetype;

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToProperty>();
    }

    public override void InitializeFromData(Archetype archetype)
    {
        transform.localScale = Vector3.one;
        m_archetype = archetype;
        m_bindables.ForEach(x => x.BindToProperty(archetype));
    }

    public override void ResetForPool()
    {
        m_archetype = null;
        m_bindables.ForEach(x => x.UnBind());
        ShowAsEquiped(false);
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

    public void ShowAsEquiped(bool isEquiped)
    {
        m_isEquipedContent.SetActive(isEquiped);
    }
}
