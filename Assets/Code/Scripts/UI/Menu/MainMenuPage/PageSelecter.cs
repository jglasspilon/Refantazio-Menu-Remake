using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSelecter : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField]
    private EMenuPages m_targetPage;

    [SerializeField]
    private MainMenuPage m_parentPage;

    [SerializeField]
    private TextMeshPro m_label;

    [SerializeField]
    private GameObject m_selectionSplotch, m_subtitleContainer;

    [SerializeField]
    private Color m_labelColorActive, m_labelColorInactive;

    public EMenuPages TargetPage {  get { return m_targetPage; } }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_parentPage.SetPageIndex((int)m_targetPage);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_parentPage.Confirm();
    }

    public void OnPageIndexChanged(int pageIndex)
    {
        bool isActive = pageIndex == ((int)m_targetPage);
        m_label.color = isActive ? m_labelColorActive : m_labelColorInactive;
        m_selectionSplotch.SetActive(isActive);
        m_subtitleContainer.SetActive(isActive);
    }
}
