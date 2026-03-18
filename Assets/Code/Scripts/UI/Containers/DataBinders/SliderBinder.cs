using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderBinder : PropertyBinder
{
    [Header("Slider Settings")]
    [SerializeField] private float m_proportionFactor;
    [SerializeField] private AnimationCurveAsset m_animationCurve;

    private Slider m_slider;
    CancellationTokenSource cts;

    private void Awake()
    {
        m_slider = GetComponent<Slider>();
    }

    protected void Apply(int value)
    {
        float proportrion = Mathf.Abs(value * m_proportionFactor);
        cts?.Cancel();
        cts = new CancellationTokenSource();
        Helper.Animation.LerpSlider(m_slider, proportrion, m_animationCurve.Curve, cts.Token);
    }

    protected void Apply(float value)
    {
        float proportrion = Mathf.Abs(value * m_proportionFactor);
        cts?.Cancel();
        cts = new CancellationTokenSource();
        Helper.Animation.LerpSlider(m_slider, proportrion, m_animationCurve.Curve, cts.Token);
    }

    protected override void Apply(object value)
    {
        Logger.Log($"Failed to bind property to Slider {gameObject.name}. Property type {value.GetType()} is not supported.", m_logProfile);
    }
}
