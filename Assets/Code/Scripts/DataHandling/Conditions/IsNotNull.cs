using UnityEngine;

[CreateAssetMenu(menuName = "Data/Condition/Is Not Null Condition")]
public class IsNotNull : Condition
{
    public override bool IsMet(object value, out string message)
    {
        message = null;
        return value != null;
    }
}
