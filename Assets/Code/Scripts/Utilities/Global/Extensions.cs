using System;
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
        float castedValue = value;
        float mapped = castedValue.Map(originalMin, originalMax, newMin, newMax);
        return (int)Mathf.Round(mapped);
    }

    public static float GetDuration(this AnimationCurve curve)
    {
        if(curve.length == 0)
        {
            Debug.LogError("Provided animation curve has no keyframes. Duration can't be calculated");
            return 0;
        }

        return curve.keys[curve.length - 1].time;
    }

    public static void ForEach<T>(this T[] array, Action<T> action)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));
        if (action == null) throw new ArgumentNullException(nameof(action));

        for (int i = 0; i < array.Length; i++)
            action(array[i]);
    }
}
