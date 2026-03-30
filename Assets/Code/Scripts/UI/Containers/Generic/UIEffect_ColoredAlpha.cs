using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class UIEffect_ColoredAlpha : UIEffect
{
    [SerializeField]
    private GameObject m_owner;
    
    [SerializeField]
    private MaskableGraphic m_appliedTo;

    [SerializeField]
    private Color m_effectColor;

    [SerializeField]
    private AnimationCurveAsset m_alphaCurve;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private CancellationTokenSource cts;
    private Color m_originalColor;

    private void Awake()
    {
        if (m_appliedTo == null)
        {
            Logger.LogError($"Failed to register original color for {gameObject.name} owned by {m_owner}. No Asset provided to apply colored alpha effect.", m_logProfile);
            return;
        }

        m_originalColor = m_appliedTo.color;
    }

    public override void PlayEffect()
    {
        if(m_appliedTo == null)
        {
            Logger.LogError($"Failed to play effect on {gameObject.name} owned by {m_owner}. No Asset provided to apply colored alpha effect.", m_logProfile);
            return;
        }

        StopEffect();
        cts = new CancellationTokenSource();
        ApplyAlphaFromCurve(cts.Token);
    }

    public override void StopEffect()
    {
        cts?.Cancel();
        m_appliedTo.color = m_originalColor;
    }

    private async UniTask ApplyAlphaFromCurve(CancellationToken token)
    {
        m_appliedTo.color = m_effectColor;
        await Helper.Animation.ApplyAlphaToGraphicFromCurve(m_appliedTo, m_alphaCurve.Curve, token);
        m_appliedTo.color = m_originalColor; 
    }
}
