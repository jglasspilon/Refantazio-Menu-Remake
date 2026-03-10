using UnityEngine;
using UnityEngine.UI;

public class StaggeredScrollRect : ScrollRect
{
    [SerializeField] 
    private float m_horizontalFactor = 0.1f;

    private Vector2 ScrollDirection => new Vector2(m_horizontalFactor, 1f).normalized;

    public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (!IsActive())
            return;

        Vector2 delta = eventData.delta;

        float magnitude = Vector2.Dot(delta, ScrollDirection);
        Vector2 projected = ScrollDirection * magnitude;
        base.OnDrag(eventData);
        content.anchoredPosition += projected;
    }

    public override void OnScroll(UnityEngine.EventSystems.PointerEventData data)
    {
        if (!IsActive())
            return;

        float scroll = data.scrollDelta.y;
        Vector2 projected = ScrollDirection * scroll * scrollSensitivity;
        content.anchoredPosition += projected;
    }
}