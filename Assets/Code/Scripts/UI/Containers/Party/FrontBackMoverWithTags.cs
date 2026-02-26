using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class FrontBackMoverWithTags : FrontBackMover
{
    [SerializeField]
    private GameObject m_frontTag, m_backTag;

    [SerializeField]
    private GameObject[] m_onSelectedContent;

    [SerializeField]
    private Transform[] m_resizeOnSelected;

    private Vector3 m_defaultScale;

    private const float SELECTED_SCALE_FACTOR = 1.0f;
    private const float UNSELECTED_SCALE_FACTOR = 0.5f;

    private void Awake()
    {
        m_defaultScale = transform.localScale;
    }

    public override void SetEnable(bool enabled)
    {
        m_frontTag.SetActive(enabled);
        m_backTag.SetActive(enabled);
        base.SetEnable(enabled);    
    }

    public override void SetAsSelected(bool isSelected)
    {
        float scaleFactor = isSelected ? SELECTED_SCALE_FACTOR : UNSELECTED_SCALE_FACTOR;
        m_onSelectedContent.ForEach(x => x.SetActive(isSelected));
        m_resizeOnSelected.ForEach(x => x.localScale = m_defaultScale * scaleFactor);
    }

    public override void SetPosition(EBattlePosition position)
    {
        if (!m_isEnabled)
            return;

        Vector2 targetPosition = position == EBattlePosition.Front ? m_frontPosition : m_backPosition;
        m_frontTag.SetActive(position == EBattlePosition.Front);
        m_backTag.SetActive(position == EBattlePosition.Back);

        cts?.Cancel();
        cts = new CancellationTokenSource();
        LerpPosition(targetPosition, cts.Token);
    }
}
