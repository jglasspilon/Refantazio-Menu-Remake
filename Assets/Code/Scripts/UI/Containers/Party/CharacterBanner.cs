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

    public Character Character {  get { return m_character; } }

    public override void InitializeFromData(Character data)
    {
        m_character = data;
        transform.localScale = Vector3.one;
    }

    public override void ResetForPool()
    {
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

