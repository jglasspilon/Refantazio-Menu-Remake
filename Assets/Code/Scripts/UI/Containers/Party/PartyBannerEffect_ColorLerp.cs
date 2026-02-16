using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class PartyBannerEffect_ColoreLerp : PartyBannerEffect
{
    [SerializeField]
    private GameObject m_owner;
    
    [SerializeField]
    private MaskableGraphic m_appliedTo;

    [SerializeField]
    private Color m_effectColor;

    [SerializeField]
    private AnimationCurveAsset m_animCurve;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private CancellationTokenSource cts;
    private Color m_originalColor;

    private void Awake()
    {
        if (m_appliedTo == null)
        {
            Logger.LogError($"Failed to register original color for {gameObject.name} effect owned by {m_owner}. No Asset provided to apply colored alpha effect.", m_logProfile);
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

        cts?.Cancel();
        cts = new CancellationTokenSource();
        ApplyLerpFromCurve(cts.Token);
    }

    public override void StopEffect()
    {
        cts?.Cancel();
        m_appliedTo.color = m_originalColor;
    }

    private async UniTask ApplyLerpFromCurve(CancellationToken token)
    {
        float duration = m_animCurve.GetDuration();
        float timer = 0f;

        while(timer < duration)
        {
            Color transformedColor = Color.Lerp(m_originalColor, m_effectColor, m_animCurve.Evaluate(timer));
            m_appliedTo.color = transformedColor;

            await UniTask.Yield(PlayerLoopTiming.Update, token);
            timer += Time.deltaTime;
        }

        m_appliedTo.color = m_originalColor;
    }
}
