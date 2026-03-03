using System;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class CharacterBanner : PoolableObjectFromData<Character>, ISelectable
{
    public event Action<bool> OnSetAsSelected, OnSetAsSelectable;
    protected Character m_character;
    protected IBindableToCharacter[] m_bindables;

    public Character Character {  get { return m_character; } }

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToCharacter>(true);
    }

    public override void InitializeFromData(Character character)
    {
        m_character = character;
        transform.localScale = Vector3.one;
        SetAsSelected(false);
        m_bindables.ForEach(x => x.BindToCharacter(character));
    }

    public override void ResetForPool()
    {
        m_bindables.ForEach(x => x.Unbind());
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

