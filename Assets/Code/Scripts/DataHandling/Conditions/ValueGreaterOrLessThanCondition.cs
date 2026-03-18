using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Condition/Greater or Less Than Condition")]
public class ValueGreaterOrLessThanCondition : Condition
{
    [SerializeField] private float m_toCompare;
    [SerializeField] private bool m_lessThan;
    [SerializeField] private bool m_canEqual;
    public override bool IsMet(object value, out string message)
    {
        message = null;

        if (value is IConvertible)
        {
            try
            {
                double numeric = Convert.ToDouble(value);
                double compare = Convert.ToDouble(m_toCompare);

                if (m_lessThan)
                    return m_canEqual ? numeric <= compare : numeric < compare;

                return m_canEqual ? numeric >= compare : numeric > compare;
            }
            catch (Exception)
            {
                
            }
        }

        message = $"Provided value of type {value.GetType()} is not a supported numeric type.";
        return false;
    }
}
