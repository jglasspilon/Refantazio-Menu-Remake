using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class AnimatedMover : MonoBehaviour
{
    [SerializeField] private float m_targetX, m_targetY;
    [SerializeField] private AnimationCurve m_normalizedHorizontalCurve, m_normalizedVeritcalCurve;
    [SerializeField] private bool m_autoMove;

    private float m_startX, m_startY;
    private CancellationTokenSource m_cts;
    private bool m_isInitialized;

    private void OnEnable()
    {
        Initialize();

        if (m_autoMove)
        {
            m_cts?.Cancel();
            m_cts = new CancellationTokenSource();
            AutoMove(m_cts.Token);
        }
    }

    private void OnDisable()
    {
        m_cts?.Cancel();
        ResetMover();
    }

    public void Initialize()
    {
        if (m_isInitialized)
            return;

        m_startX = transform.localPosition.x;
        m_startY = transform.localPosition.y;
        m_isInitialized = true;
    }

    public void MoveIn()
    {
        m_cts?.Cancel();
        m_cts = new CancellationTokenSource();
        LerpPosition(m_targetX, m_targetY, m_cts.Token);
    }

    public async UniTask MoveInAsync()
    {
        m_cts?.Cancel();
        m_cts = new CancellationTokenSource();
        await LerpPosition(m_targetX, m_targetY, m_cts.Token);
    }

    public void MoveOut()
    {
        m_cts?.Cancel();
        m_cts = new CancellationTokenSource();
        LerpPosition(m_startX, m_startY, m_cts.Token);
    }

    public async UniTask MoveOutAsync()
    {
        m_cts?.Cancel();
        m_cts = new CancellationTokenSource();
        await LerpPosition(m_startX, m_startY, m_cts.Token);
    }

    public void ResetMover()
    {
        m_cts?.Cancel();
        transform.localPosition = new Vector2(m_startX, m_startY);
    }

    private async UniTask LerpPosition(float endX, float endY, CancellationToken token)
    {
        float oldX = transform.localPosition.x;
        float oldY = transform.localPosition.y;
        float duration = Mathf.Max(m_normalizedHorizontalCurve.GetDuration(), m_normalizedVeritcalCurve.GetDuration());
        float timer = 0f;

        try
        {
            while (timer < duration)
            {
                token.ThrowIfCancellationRequested();
                Vector2 newPosition = new Vector2();
                newPosition.x = Mathf.Lerp(oldX, endX, m_normalizedHorizontalCurve.Evaluate(timer));
                newPosition.y = Mathf.Lerp(oldY, endY, m_normalizedVeritcalCurve.Evaluate(timer));
                transform.localPosition = newPosition;
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                timer += Time.deltaTime;
            }
        }
        catch (OperationCanceledException)
        {

        }
    }

    private async UniTask AutoMove(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            transform.localPosition = new Vector2(m_startX, m_startY);
            await LerpPosition(m_targetX, m_targetY, token);            
        }
    }
}
