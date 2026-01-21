using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuPage : MenuPage
{
    [SerializeField]
    private int m_startPageIndex;

    private int m_currentPageIndex;

    private void Awake()
    {
        m_currentPageIndex = m_startPageIndex;
    }

    public override void ResetPage()
    {
        m_currentPageIndex = m_startPageIndex;
    }
}
