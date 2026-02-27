using System;
using UnityEngine;

[Serializable]
public class StatModifier
{
    [SerializeField]
    private EStatType m_statType;

    [SerializeField]
    private int m_amount;

    public EStatType Type => m_statType;
    public int Amount => m_amount;
}
