using Unity.VisualScripting;
using UnityEngine;

public static class Extensions
{
    public static Vector3 Multiply(this Vector3 vector1, Vector3 vector2)
    {
        return Vector3.Scale(vector1, vector2);
    }

    public static float Map(this float value, float originalMin, float originalMax, float newMin, float newMax)
    {
        float proportion = Mathf.InverseLerp(originalMin, originalMax, value);
        return Mathf.Lerp(newMin, newMax, proportion);
    }

    public static int Map(this int value, float originalMin, float originalMax, float newMin, float newMax)
    {
        float mapped = value.Map(originalMin, originalMax, newMin, newMax);
        return (int)Mathf.Round(mapped);
    }
}
