using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class PartyBanner : PoolableObjectFromData<Character>, ISelectable
{    
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

    public abstract void SetAsSelected(bool value);
    public abstract void SetAsSelectable(bool selectable);
    public abstract void PauseSelection();
}

