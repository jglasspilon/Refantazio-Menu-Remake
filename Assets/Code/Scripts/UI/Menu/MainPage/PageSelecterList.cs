using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PageSelecterList : MonoBehaviour
{
    [SerializeField]
    private LoggingProfile m_logProfile;

    private Animator m_anim;
    private MainMenuPage m_parentPage;
    private Dictionary<int, PageSelecter> m_pageSelectors;    

    public int NumSelecters => m_pageSelectors.Values.Count(); 

    private void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_pageSelectors = GetComponentsInChildren<PageSelecter>(true).ToDictionary(x => (int)x.TargetPage, x => x);
        m_parentPage = GetComponentInParent<MainMenuPage>();
    }

    private void OnEnable()
    {
        m_parentPage.OnPageIndexChanged += UpdatePageSelecters;
        UpdatePageSelecters(m_parentPage.CurrentPageIndex);
    }

    private void OnDisable()
    {
        m_parentPage.OnPageIndexChanged -= UpdatePageSelecters;
    }

    public bool IsSelectorAtIndexSelectable(int pageIndex)
    {
        if(!m_pageSelectors.TryGetValue(pageIndex, out PageSelecter selecter))
        {
            string msg = $"Page selecter at page index {pageIndex} does not exist.";
            Logger.LogError(msg, gameObject, m_logProfile);
            return false;
        }

        return selecter.CanSelect;
    }

    private void UpdatePageSelecters(int pageIndex)
    {
        m_anim.SetInteger("PageIndex", pageIndex);
        foreach (PageSelecter page in m_pageSelectors.Values)
        {
            page.OnPageIndexChanged(pageIndex);
        }
    }
}
