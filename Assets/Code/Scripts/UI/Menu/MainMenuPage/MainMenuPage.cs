using System;
using UnityEngine;

public class MainMenuPage : MenuPage
{
    public event Action<int> OnPageIndexChanged;

    [SerializeField]
    private int m_startPageIndex;

    [SerializeField]
    private PageSelecterList m_pageSelecterList;

    [SerializeField]
    private GameObject m_projectionContent;

    private Menu m_menuParent;
    private int m_currentPageIndex;
    private const int MIN_INDEX = 1;
    private const int MAX_INDEX = 9;

    public int CurrentPageIndex {  get { return m_currentPageIndex == 0 ? m_startPageIndex : m_currentPageIndex; } } 

    private void Awake()
    {
        ResetPage();
        m_menuParent = GetComponentInParent<Menu>();
    }

    private void OnEnable()
    {
        m_pageSelecterList.gameObject.SetActive(true);
        m_projectionContent.SetActive(true);
    }

    private void OnDisable()
    {
        m_pageSelecterList.gameObject.SetActive(false);
        m_projectionContent.SetActive(false);
    }

    public override void CycleUp()
    {
        SetPageIndex(m_currentPageIndex - 1);
    }

    public override void CycleDown()
    {
        SetPageIndex(m_currentPageIndex + 1);
    }

    public override void Confirm()
    {
        EMenuPages nextPage = (EMenuPages)m_currentPageIndex;
        m_menuParent.ChangePageAsync(nextPage);
    }

    public override void ResetPage()
    {
        m_currentPageIndex = m_startPageIndex;
    }

    public void SetPageIndex(int pageIndex)
    {
        int newIndex = pageIndex;

        if (newIndex < MIN_INDEX)
            newIndex = MAX_INDEX;

        if (newIndex > MAX_INDEX)
            newIndex = MIN_INDEX;

        m_currentPageIndex = newIndex;       
        OnPageIndexChanged?.Invoke(m_currentPageIndex);
    }
}
