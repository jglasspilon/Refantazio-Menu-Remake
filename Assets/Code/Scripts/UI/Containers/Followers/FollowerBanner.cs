using System;
using UnityEngine;
using UnityEngine.UI;

public class FollowerBanner : PoolableObjectFromData<Follower>, ISelectable
{
    [SerializeField] private Animator m_anim;

    public event Action<bool> OnSetAsSelected, OnSetAsSelectable;
    protected Follower m_follower;
    protected IBindableToProperty[] m_bindables;

    public Follower Follower {  get { return m_follower; } }

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToProperty>(true);
    }

    public override void InitializeFromData(Follower follower)
    {
        m_follower = follower;
        transform.localScale = Vector3.one;
        SetAsSelected(false);
        m_bindables.ForEach(x => x.BindToProperty(m_follower));
    }

    public override void ResetForPool()
    {
        m_bindables.ForEach(x => x.UnBind());
        m_follower = null;
    }

    public virtual void SetAsSelectable(bool selectable)
    {
        OnSetAsSelectable?.Invoke(selectable);
    }

    public virtual void SetAsSelected(bool value)
    {
        m_anim.SetBool("IsSelected", value);
        OnSetAsSelected?.Invoke(value);
        m_follower?.MarkAsSeen();
    }
    
    public virtual void PauseSelection()
    {

    }
}

