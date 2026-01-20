using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Menu : MonoBehaviour
{
    public event Action OnPageChange, OnPageChangeComplete;
    private Dictionary<EMenuPages, MenuPage> m_menuPages;
    private MenuPage m_activePage;
    private MenuPage m_nextPage;
    private int m_pageCount;

    private void Awake()
    {
        m_menuPages = GetComponentsInChildren<MenuPage>().ToDictionary(x => x.PageName, x => x);
        foreach(MenuPage page in m_menuPages.Values)
        {
            page.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    public void Launch()
    {
        InputManager.Instance.SetInputMap(EInputMap.Menu);
        gameObject.SetActive(true);
        ChangePage(EMenuPages.Main);
    }

    public void ChangePage(EMenuPages page)
    {
        if(!m_menuPages.ContainsKey(page))
        {
            Debug.LogError($"Failed to open menu page '{page}'. '{page}' page is was not present as a child page of the Menu Object.");
            return;
        }

        OnPageChange?.Invoke();
        m_nextPage = m_menuPages[page];

        if(m_activePage == null)
        {
            OpenNextPage();
            return;
        }

        m_activePage.OnClosed += OpenNextPage;
        m_activePage.Close();       

        void OpenNextPage()
        {
            if (m_activePage != null)
            {
                m_activePage.OnClosed -= OpenNextPage;
                m_activePage.gameObject.SetActive(false);
            }

            m_activePage = m_nextPage;
            m_activePage.gameObject.SetActive(true);
            m_activePage.OnOpened += PageChangeComplete;
            m_activePage.Open(m_pageCount);
            m_pageCount++;
        }

        void PageChangeComplete()
        {
            m_activePage.OnOpened -= PageChangeComplete;
            OnPageChangeComplete?.Invoke();
        }
    }

    public void Close()
    { 
        if(m_activePage == null)
        {
            Teardown();
            return;
        }

        m_activePage.OnClosed += Teardown;
        m_activePage.Close();

        void Teardown()
        {
            if (m_activePage != null)
            {
                m_activePage.OnClosed -= Teardown;
                m_activePage.gameObject.SetActive(false);
            }

            InputManager.Instance.SetInputMap(EInputMap.Player);
            gameObject.SetActive(false);
            m_activePage = null;
            m_pageCount = 0;
        }
    }    
}

public enum EMenuPages
{
    Main,
    Skill,
    Item,
    Equipment,
    Party,
    Follower,
    Quest,
    Calendar,
    Memorandum,
    System
}
