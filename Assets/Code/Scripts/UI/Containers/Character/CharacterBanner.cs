using System;
using System.Linq;
using UnityEngine;

public class CharacterBanner : PoolableObjectFromData<Character>, ISelectable
{
    public event Action<bool> OnSetAsSelected, OnSetAsSelectable;
    protected Character m_character;
    protected IBindableToProperty[] m_bindables;

    public Character Character {  get { return m_character; } }

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToProperty>(true);
    }

    public override void InitializeFromData(Character character)
    {
        m_character = character;
        transform.localScale = Vector3.one;
        SetAsSelected(false);
        m_bindables.ForEach(x => x.BindToProperty(character));
    }

    public override void ResetForPool()
    {
        m_bindables.ForEach(x => x.UnBind());
        m_character = null;
    }

    public virtual void SetAsSelectable(bool selectable)
    {
        OnSetAsSelectable?.Invoke(selectable);
    }

    public virtual void SetAsSelected(bool value)
    {
        OnSetAsSelected?.Invoke(value);
    }
    
    public virtual void PauseSelection()
    {

    }
}

