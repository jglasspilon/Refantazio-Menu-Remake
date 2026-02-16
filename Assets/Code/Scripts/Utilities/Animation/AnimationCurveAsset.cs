using UnityEngine;

[CreateAssetMenu(menuName = "Animation/Animation Curve")]
public class AnimationCurveAsset : ScriptableObject
{
    public AnimationCurve Curve;

    public float Evaluate(float time)
    {
        return Curve.Evaluate(time);
    }

    public float GetDuration()
    {
        return Curve.GetDuration();
    }
}
