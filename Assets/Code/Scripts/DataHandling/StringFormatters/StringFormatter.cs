using UnityEngine;

public abstract class StringFormatter : ScriptableObject
{
    public abstract string Format(object value, out string message);
}
