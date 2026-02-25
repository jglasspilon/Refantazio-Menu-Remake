using System;
using UnityEngine;

[Serializable]
public class StatModifier
{
    [SerializeField]
    private StatType m_statType;

    [SerializeField]
    private int m_amount;

    public StatType Type => m_statType;
    public int Amount => m_amount;
}
