using System;
using UnityEngine;

[Serializable]
public class Resource
{
    public event Action<int, float, int> OnResourceChange;
    public event Action<bool> OnEmpty;

    [SerializeField]
    private int m_current;

    [SerializeField]
    private int m_max;

    public int Current => m_current;
    public float CurrentProportion => Mathf.InverseLerp(0, m_max, m_current);
    public int Max => m_max;

    public void SetMax(int newMax, bool fill)
    {
        m_current = m_current.Map(0, m_max, 0, newMax);
        m_max = newMax;

        if (fill) 
        {
            m_current = m_max;
            OnResourceChange?.Invoke(m_current, CurrentProportion, 0);
        }
    }

    public void Apply(int amount)
    {
        int previous = m_current;
        m_current = Mathf.Clamp(m_current + amount, 0, m_max);
        int delta = m_current - previous;
        OnResourceChange?.Invoke(m_current, CurrentProportion, delta);

        if(previous == 0 && m_current > 0)
        {
            OnEmpty?.Invoke(false);
            return;
        }

        if(m_current == 0)
        {
            OnEmpty?.Invoke(true);
        }
    }
}
