using System;
using UnityEngine;

[Serializable]
public class ContentFramer
{
    [SerializeField] 
    private StaggeredScrollRect m_scrollRect;

    [SerializeField] 
    private RectTransform m_viewport;

    [Tooltip("How much vertical delta to nudge per step when correcting visibility.")]
    [SerializeField] 
    private float m_nudgeAmount = 20f;

    public void EnsureVisible(RectTransform target)
    {
        if (m_scrollRect == null || m_viewport == null || target == null)
            return;

        // Convert positions into viewport-local space
        Vector3 targetTop = m_viewport.InverseTransformPoint(GetWorldTop(target));
        Vector3 targetBottom = m_viewport.InverseTransformPoint(GetWorldBottom(target));

        float viewportHeight = m_viewport.rect.height;

        // In viewport-local space:
        // y = 0 is the center, +y is up, -y is down.
        float halfHeight = viewportHeight * 0.5f;

        bool aboveTop = targetTop.y > halfHeight;
        bool belowBottom = targetBottom.y < -halfHeight;

        if (!aboveTop && !belowBottom)
            return; // Already fully inside vertically

        // If above the top, nudge upward (negative vertical delta)
        if (aboveTop)
        {
            m_scrollRect.ScrollByDelta(m_nudgeAmount);
            EnsureVisible(target);
        }
        // If below the bottom, nudge downward (positive vertical delta)
        else if (belowBottom)
        {
            m_scrollRect.ScrollByDelta(-m_nudgeAmount);
            EnsureVisible(target);
        }
    }

    private Vector3 GetWorldTop(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return corners[1]; // top-left
    }

    private Vector3 GetWorldBottom(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return corners[0]; // bottom-left
    }
}
