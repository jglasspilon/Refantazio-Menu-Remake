using Cysharp.Threading.Tasks;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class AnimatedMover : MonoBehaviour
{
    [SerializeField]
    private float m_targetX, m_targetY;

    [SerializeField]
    private AnimationCurve m_normalizedHorizontalCurve, m_normalizedVeritcalCurve;

    private float m_startX, m_startY;

    private void Awake()
    {
        m_startX = transform.localPosition.x;
        m_startY = transform.localPosition.y;
    }

    public async UniTask MoveIn()
    {
        await LerpPosition(m_targetX, m_targetY);
    }

    public async UniTask MoveOut()
    {
        await LerpPosition(m_startX, m_startY);
    }

    private async UniTask LerpPosition(float endX, float endY)
    {
        float oldX = transform.localPosition.x;
        float oldY = transform.localPosition.y;
        float duration = Mathf.Max(m_normalizedHorizontalCurve.GetDuration(), m_normalizedVeritcalCurve.GetDuration());
        float timer = 0f;

        while (timer < duration)
        {
            Vector2 newPosition = new Vector2();
            newPosition.x = Mathf.Lerp(oldX, endX, m_normalizedHorizontalCurve.Evaluate(timer));
            newPosition.y = Mathf.Lerp(oldY, endY, m_normalizedVeritcalCurve.Evaluate(timer));
            transform.localPosition = newPosition;
            await UniTask.Yield(PlayerLoopTiming.Update);
            timer += Time.deltaTime;
        }

        transform.localPosition = new Vector2(endX, endY);
    }
}
