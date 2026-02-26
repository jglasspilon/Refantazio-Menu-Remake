using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class FrontBackMover : MonoBehaviour
{
    [SerializeField]
    private bool m_enabledByDefault;

    [SerializeField]
    protected Vector2 m_frontPosition, m_backPosition;

    [SerializeField]
    protected AnimationCurveAsset m_curve;

    protected bool m_isEnabled;
    protected EBattlePosition m_position = EBattlePosition.Undetermined;
    protected CancellationTokenSource cts;

    private void Awake()
    {
        m_isEnabled = m_enabledByDefault;
    }

    public virtual void SetEnable(bool enabled)
    {
        m_isEnabled = enabled;

        if(!enabled)
            transform.localPosition = m_frontPosition;
    }

    public virtual void SetAsSelected(bool isSelected)
    {

    }

    public virtual void SetPosition(EBattlePosition position)
    {
        if (!m_isEnabled)
            return;

        Vector2 targetPosition = position == EBattlePosition.Front ? m_frontPosition : m_backPosition;

        cts?.Cancel();
        cts = new CancellationTokenSource();
        LerpPosition(targetPosition, cts.Token);
    }

    protected async UniTask LerpPosition(Vector2 target, CancellationToken token)
    {
        Vector2 old = transform.localPosition;
        float duration = m_curve.GetDuration();
        float timer = 0f;

        while (timer < duration)
        {
            Vector2 newPosition = Vector2.Lerp(old, target, m_curve.Evaluate(timer));
            transform.localPosition = newPosition;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
            timer += Time.deltaTime;
        }

        transform.localPosition = target;
    }
}
