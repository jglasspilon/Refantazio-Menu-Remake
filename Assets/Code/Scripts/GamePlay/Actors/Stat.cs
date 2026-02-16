using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField]
    private int m_baseValue, m_levelValue;

    [SerializeField]
    private List<GameObject> m_modifiers = new(); //TODO: Replace with modifiers once they are created

    public int BaseValue => m_baseValue;
    public int LevelValue => m_levelValue;

    public int Value => m_baseValue + m_levelValue; //TODO: Apply modifiers

    public Stat(int baseValue)
    {
        m_baseValue = baseValue;
    }

    public Stat()
    { }

    public void ApplyLevel(int levelAmount)
    {
        m_levelValue += levelAmount;
    }

    public void AddModifier(object modifier) //TODO: Replace with modifiers once they are created
    {

    }

    public void RemoveModifier(object modifier) //TODO: Replace with modifiers once they are created
    {

    }
}
