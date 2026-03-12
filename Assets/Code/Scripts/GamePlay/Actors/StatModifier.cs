using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatModifier: ISubPropertyProvider
{
    [SerializeField] private ObservableProperty<EStatType> m_statType = new ObservableProperty<EStatType>();
    [SerializeField] private ObservableProperty<int> m_amount = new ObservableProperty<int>();

    public EStatType Type => m_statType.Value;
    public int Amount => m_amount.Value;

    public StatModifier(EStatType statType, int amount)
    {
        m_statType.Value = statType;
        m_amount.Value = amount;
    }

    public IEnumerable<KeyValuePair<string, IObservableProperty>> GetSubProperties(string parentKey)
    {
        return Helper.DataHandling.GetObservableFields(this, parentKey);
    }
}
