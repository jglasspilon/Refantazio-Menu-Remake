using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Resource: ISubPropertyProvider
{
    [SerializeField] private ObservableProperty<int> m_current, m_max;

    public event Action<Resource, int> OnResourceChange;
    public event Action<bool> OnEmpty;

    public Type ValueType => typeof(Resource);
    public object UntypedValue => this;
    public int Current => m_current.Value;
    public int Max => m_max.Value;
    public float CurrentProportion => Mathf.InverseLerp(0, Max, Current);

    public Resource(int initialMax)
    {
        m_current = new ObservableProperty<int>();
        m_max = new ObservableProperty<int>
        {
            Value = initialMax
        };
    }

    public IEnumerable<KeyValuePair<string, IObservableProperty>> GetSubProperties(string parentKey)
    {
        return Helper.DataHandling.GetObservableFields(this, parentKey);
    }

    public void SetMax(int newMax, EResourceSetProcedure procedure)
    {
        m_max.Value = newMax;

        if (procedure == EResourceSetProcedure.Fill) 
        {
            m_current.Value = m_max.Value;
            return;
        }

        if(procedure == EResourceSetProcedure.Reset)
        {
            m_current.Value = 0;
            return;
        }
    }

    public void Apply(int amount)
    {
        int previous = m_current.Value;
        m_current.Value = Mathf.Clamp(m_current.Value + amount, 0, m_max.Value);
        int delta = m_current.Value - previous;
        OnResourceChange?.Invoke(this, delta);

        if(previous == 0 && m_current.Value > 0)
        {
            OnEmpty?.Invoke(false);
            return;
        }

        if(m_current.Value == 0)
        {
            OnEmpty?.Invoke(true);
        }
    }
}

public enum EResourceSetProcedure
{
    Fill,
    Reset,
    Keep
}
