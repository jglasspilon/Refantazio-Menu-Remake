using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Condition/Is Character Type Condition")]
public class IsCharacterTypeCondition : Condition
{
    [SerializeField] private ECharacterType[] m_successfulCharacterTypes;
    public override bool IsMet(object value, out string message)
    {
        message = null;

        if(value is ECharacterType characterType)
        {
            return m_successfulCharacterTypes.Contains(characterType);
        }

        if(value is Character character)
        {
            return m_successfulCharacterTypes.Contains(character.CharacterType.Value);
        }

        message = $"Provided value of type {value.GetType()} does not match expected type ECharacterType. IsCharacterType condition failed.";
        return false;
    }
}
