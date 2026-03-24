using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Stat: ISubPropertyProvider
{
    [SerializeField] private EStatType m_type;
    [SerializeField] private int m_baseValue, m_levelValue;
    [SerializeField] private ObservableProperty<int> m_final = new ObservableProperty<int>();
    [SerializeField] private ObservableProperty<int> m_raw = new ObservableProperty<int>();
    [SerializeField] private ObservableProperty<int> m_delta = new ObservableProperty<int>();
    [SerializeField] private List<StatModifier> m_modifiers = new List<StatModifier>();

    private int m_maxValue;
    public int BaseValue => m_baseValue;
    public int LevelValue => m_levelValue;
    public ObservableProperty<int> Raw => m_raw;
    public ObservableProperty<int> Final => m_final;
    public ObservableProperty<int> Delta => m_delta;

    public EStatType Type => m_type;

    public Stat(EStatType type, int baseValue, int maxValue)
    {
        m_type = type;
        m_maxValue = maxValue;
        m_baseValue = baseValue;
        Recalculate();
    }

    public IEnumerable<KeyValuePair<string, IObservableProperty>> GetSubProperties(string parentKey)
    {
        return Helper.DataHandling.GetObservableFields(this, parentKey);
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
        m_raw.Value = m_baseValue + m_levelValue;
        m_final.Value = Mathf.Clamp(m_baseValue + m_levelValue + m_modifiers.Sum(x => x.Amount), 0, m_maxValue);
        m_delta.Value = m_final.Value - m_raw.Value;
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
