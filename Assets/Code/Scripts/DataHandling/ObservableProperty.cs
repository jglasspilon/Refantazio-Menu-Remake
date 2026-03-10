using System;
using UnityEngine;

[Serializable]
public class ObservableProperty<T>: IObservableProperty
{
    [SerializeField]
    private T m_value;

    public event Action<T> OnChanged;

    public T Value
    {
        get => m_value;
        set
        {
            if(!Equals(m_value, value))
            {
                m_value = value;
                OnChanged?.Invoke(m_value);
            }
        }
    }

    public Type ValueType => typeof(T);
    public object UntypedValue => Value;
}
