using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Stat: ObservableProperty<int>
{
    [SerializeField] private EStatType m_type;
    [SerializeField] private int m_baseValue, m_levelValue;
    [SerializeField] private List<StatModifier> m_modifiers = new List<StatModifier>();

    public int BaseValue => m_baseValue;
    public int LevelValue => m_levelValue;

    public EStatType Type => m_type;

    public Stat(EStatType type, int baseValue)
    {
        m_type = type;
        m_baseValue = baseValue;
        Recalculate();
    }

    public void AddToBase(int amount)
    {
        m_baseValue += amount;
        Recalculate();
    }

    public void ApplyLevel(int amount)
    {
        m_levelValue += amount;
        Recalculate();
    }

    public void AddModifier(StatModifier modifier)
    {
        if (modifier == null)
            return;

        m_modifiers.Add(modifier);
        Recalculate();
    }

    public void RemoveModifier(StatModifier modifier)
    {
        if (modifier == null)
            return;

        m_modifiers.Remove(modifier);
        Recalculate();
    }

    private void Recalculate()
    {
        Value = Mathf.Clamp(m_baseValue + m_levelValue + m_modifiers.Sum(x => x.Amount), 0, 99);
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
