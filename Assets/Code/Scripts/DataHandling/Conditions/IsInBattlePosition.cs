using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Condition/Is In Battle Position Condition")]
public class IsInBattlePosition : Condition
{
    [SerializeField] private EBattlePosition[] m_acceptedPositions;
    public override bool IsMet(object value, out string message)
    {
        message = null;

        if(value is not EBattlePosition position)
        {
            message = $"Provided value of type {value.GetType()} does not match expected type EBattlePosition. IsInBattlePosition condition failed.";
            return false;
        }

        return m_acceptedPositions.Contains(position);
    }
}
