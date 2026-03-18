using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class Fader : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve m_inCurve, m_outCurve;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private MaskableGraphic m_graphic;
    private CanvasGroup m_canvasGroup;
    private CancellationTokenSource cts;

    private void Awake()
    {
        m_graphic = GetComponent<MaskableGraphic>();
        m_canvasGroup = GetComponent<CanvasGroup>();

        if(m_graphic == null && m_canvasGroup == null)
        {
            Logger.LogError($"No maskable graphic or canvas group found on {gameObject.name} for fader. Fading will be disabled.", m_logProfile);
        }
    }

    public async UniTask FadeIn()
    {
        if (m_graphic != null)
        {
            await FadeGraphic(m_inCurve);
            return;
        }

        if (m_canvasGroup != null)
        {
            await FadeCanvasGroup(m_inCurve);
            return;
        }
    }

    public async UniTask FadeOut()
    {
        if (m_graphic != null)
        {
            await FadeGraphic(m_outCurve);
            return;
        }

        if (m_canvasGroup != null)
        {
            await FadeCanvasGroup(m_outCurve);
            return;
        }
    }

    public void ResetFader(bool visible)
    {
        if (m_graphic != null)
        {
            Color color = m_graphic.color;
            color.a = visible ? 1 : 0;

            m_graphic.color = color;
            return;
        }

        if(m_canvasGroup != null)
        {
            m_canvasGroup.alpha = visible ? 1 : 0;
        }
    }

    private async UniTask FadeGraphic(AnimationCurve curve)
    {
        cts?.Cancel();
        cts = new CancellationTokenSource();
        await Helper.Animation.ApplyAlphaToGraphicFromCurve(m_graphic, curve, cts.Token);
    }

    private async UniTask FadeCanvasGroup(AnimationCurve curve)
    {
        cts?.Cancel();
        cts = new CancellationTokenSource();
        await Helper.Animation.ApplyAlphaToCanvasGroupFromCurve(m_canvasGroup, curve, cts.Token);
    }
}
