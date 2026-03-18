using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Condition/Equal To Condition")]
public class ValueEqualCondition : Condition
{
    [SerializeField] private float m_toCompare;
    [SerializeField] private bool m_notEqual;

    public override bool IsMet(object value, out string message)
    {
        message = null;

        if (value is IConvertible)
        {
            try
            {
                double numeric = Convert.ToDouble(value);
                double compare = Convert.ToDouble(m_toCompare);

                if (m_notEqual)
                    return numeric != compare;

                return numeric == compare;
            }
            catch (Exception)
            {
                
            }
        }

        message = $"Provided value of type {value.GetType()} is not a supported numeric type.";
        return false;
    }
}
