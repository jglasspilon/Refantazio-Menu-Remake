using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Stat
{
    public event Action<Stat> OnValueChange;

    [SerializeField]
    private EStatType m_type;
    
    [SerializeField]
    private int m_baseValue, m_levelValue;

    [SerializeField]
    private List<StatModifier> m_modifiers = new List<StatModifier>();

    public int BaseValue => m_baseValue;
    public int LevelValue => m_levelValue;

    public EStatType Type => m_type;
    public int Value => m_baseValue + m_levelValue + m_modifiers.Sum(x => x.Amount);

    public Stat(EStatType type, int baseValue)
    {
        m_type = type;
        m_baseValue = baseValue;
    }

    public void AddToBase(int amount)
    {
        m_baseValue += amount;
        OnValueChange?.Invoke(this);
    }

    public void ApplyLevel(int amount)
    {
        m_levelValue += amount;
        OnValueChange?.Invoke(this);
    }

    public void AddModifier(StatModifier modifier) //TODO: Replace with modifiers once they are created
    {
        m_modifiers.Add(modifier);
        OnValueChange?.Invoke(this);
    }

    public void RemoveModifier(StatModifier modifier) //TODO: Replace with modifiers once they are created
    {
        m_modifiers.Remove(modifier);
        OnValueChange?.Invoke(this);
    }
}

public enum EStatType
{
    HP,
    MP,
    Strength, 
    Magic,
    Endurance, 
    Agility, 
    Luck,

    Attack,
    Hit, 
    Defence,
    Evasion
}
