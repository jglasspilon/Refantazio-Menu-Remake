using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PageSelecterList : MonoBehaviour
{  
    [SerializeField]
    private Animator m_anim;

    [SerializeField]
    private MainMenuPage m_parentPage;

    private Dictionary<int, PageSelecter> m_pageSelectors;    

    private void Awake()
    {
        m_pageSelectors = GetComponentsInChildren<PageSelecter>().ToDictionary(x => (int)x.TargetPage, x => x);
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

    private void UpdatePageSelecters(int pageIndex)
    {
        m_anim.SetInteger("PageIndex", pageIndex);
        foreach (PageSelecter page in m_pageSelectors.Values)
        {
            page.OnPageIndexChanged(pageIndex);
        }
    }
}
