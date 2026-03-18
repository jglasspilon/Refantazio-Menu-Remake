using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class OpacityPulseEffect : MonoBehaviour
{
    [SerializeField] private AnimationCurveAsset m_animationCurve;

    private MaskableGraphic m_graphic;
    private float m_defaultAlpha;
    private CancellationTokenSource m_cts;

    void Awake()
    {
        m_graphic = GetComponent<MaskableGraphic>();
        m_defaultAlpha = m_graphic.color.a;
    }

    private void OnDestroy()
    {
        m_cts?.Cancel();
    }

    private void OnEnable()
    {
        m_cts?.Cancel();
        m_cts = new CancellationTokenSource();
        PulseAnimation(m_cts.Token);
    }        

    private void OnDisable()
    {
        m_cts?.Cancel();
        Color defaultColor = m_graphic.color;
        defaultColor.a = m_defaultAlpha;
        m_graphic.color = defaultColor;
    }

    private async UniTask PulseAnimation(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await Helper.Animation.ApplyAlphaToGraphicFromCurve(m_graphic, m_animationCurve.Curve, token);
        }
    }
}
