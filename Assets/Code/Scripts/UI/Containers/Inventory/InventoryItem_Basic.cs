using UnityEngine;

public class InventoryItem_Basic : InventoryItemUI
{
    [SerializeField]
    private CanvasGroup m_alphaGroup;

    [SerializeField]
    private GameObject m_selectionSplotch, m_selectionFrame, m_shadow;

    [SerializeField]
    private Animator m_anim;

    private const float UNUSABLE_ALPHA = 0.5f;
    private const float USABLE_ALPHA = 1.0f;

    public override void SetAsSelected(bool selected)
    {
        base.SetAsSelected(selected);
        m_anim.SetBool("IsSelected", selected);
        m_selectionFrame.SetActive(false);
        m_selectionSplotch.SetActive(selected);
        m_shadow.SetActive(!selected);
    }

    public override void PauseSelection()
    {
        m_selectionFrame.SetActive(true);
        m_selectionSplotch.SetActive(false);
    }   

    public override void SetAsSelectable(bool usable)
    {
        m_alphaGroup.alpha = usable ? USABLE_ALPHA : UNUSABLE_ALPHA;
        base.SetAsSelectable(usable);
    }
}
