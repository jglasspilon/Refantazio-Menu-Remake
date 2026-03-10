using UnityEngine;
using UnityEngine.UI;

public class StaggeredVerticalLayoutGroup : VerticalLayoutGroup
{
    [SerializeField] 
    private float m_horizontalFactor = 0.1f;

    public override void SetLayoutHorizontal()
    {
        base.SetLayoutHorizontal();
    }

    public override void SetLayoutVertical()
    {
        base.SetLayoutVertical();
        ApplyHeightBasedOffsets();
    }

    private void ApplyHeightBasedOffsets()
    {
        if (rectChildren.Count == 0)
            return;

        // The top-most child after layout
        float topY = rectChildren[0].anchoredPosition.y;

        foreach (var child in rectChildren)
        {
            Vector2 pos = child.anchoredPosition;
            float verticalDistance = Mathf.Abs(pos.y - topY);
            pos.x += verticalDistance * m_horizontalFactor;
            child.anchoredPosition = pos;
        }
    }

}