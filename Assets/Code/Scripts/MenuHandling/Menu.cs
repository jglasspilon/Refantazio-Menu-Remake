using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class Menu : MonoBehaviour
{
    public event Action OnPageChange, OnPageChangeComplete;
    private Dictionary<EMenuPages, MenuPage> m_menuPages;
    private MenuPage m_activePage;
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
        ChangePageAsync(EMenuPages.Main);
    }

    public async UniTask ChangePageAsync(EMenuPages page)
    {
        if(!m_menuPages.TryGetValue(page, out MenuPage nextPage))
        {
            Debug.LogError($"Failed to open menu page '{page}'. '{page}' page is was not present as a child page of the Menu Object.");
            return;
        }

        OnPageChange?.Invoke();

        if(m_activePage != null)
        {
            await m_activePage.CloseAsync();
            m_activePage.gameObject.SetActive(false);
        }

        m_activePage = nextPage;
        m_activePage.gameObject.SetActive(true);
        await m_activePage.OpenAsync(m_pageCount);

        m_pageCount++;
        OnPageChangeComplete?.Invoke();
    }

    public void Back()
    {
        if (m_activePage != null)
        {
            if(m_activePage.PageName != EMenuPages.Main)
            {
                if (!m_activePage.TryGoBack())
                {
                    ChangePageAsync(EMenuPages.Main);
                }
                return;
            }
            m_activePage.Close();
        }

        Teardown();
    }    

    private void Teardown()
    {
        foreach (MenuPage page in m_menuPages.Values)
        {
            page.ResetPage();
        }

        InputManager.Instance.SetInputMap(EInputMap.Player);
        gameObject.SetActive(false);
        m_activePage = null;
        m_pageCount = 0;
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
