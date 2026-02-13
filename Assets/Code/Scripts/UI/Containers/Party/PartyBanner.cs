using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public abstract class PartyBanner : PoolableObject
{
    protected Character m_character;

    public Character Character {  get { return m_character; } } 

    public virtual void InitializeCharacter(Character character)
    {
        m_character = character;
        transform.localScale = Vector3.one;
    }

    public override void ResetForPool()
    {
        m_character = null;
    }
}
