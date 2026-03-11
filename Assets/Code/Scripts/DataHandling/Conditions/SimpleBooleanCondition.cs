using UnityEngine;

[CreateAssetMenu(menuName = "Data/Condition/Simple Boolean Condition")]
public class SimpleBooleanCondition : Condition
{
    [SerializeField] private bool m_isFalse;
    public override bool IsMet(object value, out string message)
    {
        message = null;

        if(value is not bool isTrue)
        {
            message = $"Provided value of type {value.GetType()} does not match expected type bool. Simple Boolean condition failed.";
            return false;
        }

        if (m_isFalse)
            isTrue = !isTrue;

        return isTrue;
    }
}
