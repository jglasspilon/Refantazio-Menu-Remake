using UnityEngine;

public static class Extensions
{
    public static Vector3 Multiply(this Vector3 vector1, Vector3 vector2)
    {
        return Vector3.Scale(vector1, vector2);
    }
}
