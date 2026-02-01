using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class MainMenuPage : MenuPage
{
    public event Action<int> OnPageIndexChanged;

    [SerializeField]
    private int m_startPageIndex;

    [SerializeField]
    private PageSelecterList m_pageSelecterList;

    private MenuManager m_menuParent;
    private int m_currentPageIndex;
    private const int MIN_INDEX = 1;

    public int CurrentPageIndex {  get { return m_currentPageIndex == 0 ? m_startPageIndex : m_currentPageIndex; } } 

    private void Awake()
    {
        ResetPage();
        m_menuParent = GetComponentInParent<MenuManager>();
    }

    public override void CycleUp()
    {
        CyclePageIndex(-1);
    }

    public override void CycleDown()
    {
        CyclePageIndex(1);
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
        int maxIndex = m_pageSelecterList.NumSelecters;

        if (newIndex < MIN_INDEX)
            newIndex = maxIndex;

        if (newIndex > maxIndex)
            newIndex = MIN_INDEX;

        m_currentPageIndex = newIndex;
        OnPageIndexChanged?.Invoke(m_currentPageIndex);
    }

    public void CyclePageIndex(int cycleAmount)
    {
        int newIndex = m_currentPageIndex + cycleAmount;
        int maxIndex = m_pageSelecterList.NumSelecters;

        if (newIndex < MIN_INDEX)
            newIndex = maxIndex;

        if (newIndex > maxIndex)
            newIndex = MIN_INDEX;

        m_currentPageIndex = newIndex;

        if (!m_pageSelecterList.IsSelectorAtIndexSelectable(m_currentPageIndex))
        {
            CyclePageIndex(cycleAmount);
            return;
        }

        OnPageIndexChanged?.Invoke(m_currentPageIndex);
    }
}
