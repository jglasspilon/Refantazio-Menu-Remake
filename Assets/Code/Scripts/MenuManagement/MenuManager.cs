using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Logger m_logger;

    public event Action OnPageChange, OnPageChangeComplete;

    private GameStateManager m_gameState;
    private Dictionary<EMenuPages, MenuPage> m_menuPages;
    private MenuPage m_activePage;
    private int m_pageCount;

    private void Awake()
    {
        m_gameState = GameStateManager.Instance;
        m_menuPages = GetComponentsInChildren<MenuPage>(true).ToDictionary(x => x.PageName, x => x);
        foreach(MenuPage page in m_menuPages.Values)
        {
            page.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        m_gameState.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        m_gameState.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(EGameState gameState)
    {
        if (gameState == EGameState.Menu)
            Launch();
        else
            Teardown();
    }

    public void Launch()
    {
        gameObject.SetActive(true);
        ChangePageAsync(EMenuPages.Main);
    }

    public async UniTask ChangePageAsync(EMenuPages page)
    {
        if(!m_menuPages.TryGetValue(page, out MenuPage nextPage))
        {
            m_logger.LogError($"Failed to open menu page '{page}'. '{page}' page was not present as a child page of the Menu Object.", gameObject);
            return;
        }

        OnPageChange?.Invoke();

        if(m_activePage != null)
        {
            await m_activePage.CloseAsync();
        }

        m_activePage = nextPage;
        await m_activePage.OpenAsync(m_pageCount);

        m_pageCount++;
        OnPageChangeComplete?.Invoke();
    }

    private void Teardown()
    {
        foreach (MenuPage page in m_menuPages.Values)
        {
            page.ResetPage();
        }

        m_activePage = null;
        m_pageCount = 0;
    }

    public void Confirm()
    {
        if (m_activePage == null)
            return;

        m_activePage.Confirm();
    }

    public void CycleUp()
    {
        if (m_activePage == null)
            return;

        m_activePage.CycleUp();
    }

    public void CycleDown()
    {
        if (m_activePage == null)
            return;

        m_activePage.CycleDown();
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
                    return;
                }
                return;
            }
            m_activePage.Close();
        }

        m_gameState.ReturnToSceneState();
    }    
}

public enum EMenuPages
{
    Main = 0,
    Skill = 1,
    Item = 2,
    Equipment = 3,
    Party = 4,
    Follower = 5,
    Quest = 6,
    Calendar = 7,
    Memorandum = 8,
    System = 9
}
