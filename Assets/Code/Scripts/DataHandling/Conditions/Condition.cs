using UnityEngine;

public abstract class Condition : ScriptableObject
{
    public abstract bool IsMet(object value, out string message);
}
