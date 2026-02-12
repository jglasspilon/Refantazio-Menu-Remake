using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private float m_launchDelay;

    [SerializeField]
    private LoggingProfile m_logProfile;

    public event Action OnPageChange, OnPageChangeComplete;

    private IGameStateManagementService m_gameState;
    private Dictionary<EMenuPages, MenuPage> m_menuPages;
    private MenuPage m_activePage;
    private int m_pageCount;

    private void Awake()
    {
        m_menuPages = GetComponentsInChildren<MenuPage>(true).ToDictionary(x => x.PageName, x => x);
        foreach(MenuPage page in m_menuPages.Values)
        {
            page.Close();
        }

        if(ObjectResolver.Instance.TryResolve(OnGameStateManagerChanged, out m_gameState))
        {
            m_gameState.OnGameStateChanged += OnGameStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (m_gameState != null)
        {
            m_gameState.OnGameStateChanged -= OnGameStateChanged;
        }
    }

    private void OnGameStateManagerChanged()
    {
        m_gameState = ObjectResolver.Instance.Resolve<IGameStateManagementService>();
        m_gameState.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(EGameStates gameState)
    {
        if (gameState == EGameStates.Menu)
            Launch();
        else
            Teardown();
    }

    public void Launch(EMenuPages launchPage = EMenuPages.Main)
    {
        gameObject.SetActive(true);
        ChangePageAsync(launchPage);
    }

    public async UniTask ChangePageAsync(EMenuPages page)
    {
        if(!m_menuPages.TryGetValue(page, out MenuPage nextPage))
        {
            string msg = $"Failed to open menu page '{page}'. '{page}' page was not found as a child of this Game Object.";
            Logger.LogError(msg, gameObject, m_logProfile);
            return;
        }

        OnPageChange?.Invoke();

        if (m_pageCount == 0)
        {
            await Helper.Timing.DelaySeconds(m_launchDelay);
        }

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

        m_gameState.ReturnToGameplaySate();
    }    
}
