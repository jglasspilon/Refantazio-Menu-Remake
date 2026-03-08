using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    public event Action OnOpening, OnOpened, OnClosing, OnClosed;

    [SerializeField]
    private EMenuPages m_pageName;

    [SerializeField]
    private PageSection m_defaultSection;

    [SerializeField]
    private Animator m_anim;

    [SerializeField]
    protected LoggingProfile m_logProfile;

    protected Stack<PageSection> m_breadcrumb = new Stack<PageSection>(); 
    protected List<PageSection> m_sectionsToReset = new List<PageSection>();
    public EMenuPages PageName => m_pageName;
    public int PageCount { get; private set; }

    #region Life-cycle Functions
    public virtual async UniTask OpenAsync(int pageCount)
    {
        OnOpening?.Invoke();

        Logger.Log($"Opening menu page '{PageName}'.", gameObject, m_logProfile);
        gameObject.SetActive(true);
        PageCount = pageCount;

        EnterSection(m_defaultSection);
        m_anim.SetInteger("PageCount", pageCount);
        m_anim.SetBool("IsActive", true);

        await Helper.Animation.WaitForCurrentPageAnimationToEnd(m_anim);

        Logger.Log($"Menu page '{PageName}' opened successfully.", gameObject, m_logProfile);
        OnOpened?.Invoke();
    }

    public virtual async UniTask CloseAsync()
    {
        OnClosing?.Invoke();
        Logger.Log($"Closing menu page '{PageName}'.", gameObject, m_logProfile);        
        m_anim.SetBool("IsActive", false);
        await Helper.Animation.WaitForCurrentPageAnimationToEnd(m_anim);

        ResetSections();
        gameObject.SetActive(false);
        Logger.Log($"Menu page '{PageName}' closed successfully.", gameObject, m_logProfile);
        OnClosed?.Invoke();
    }

    public virtual void Close()
    {
        if (m_anim.isActiveAndEnabled)
            m_anim.SetBool("IsActive", false);

        ResetSections();
        gameObject.SetActive(false);
        Logger.Log($"Menu page {PageName} closed successfully.", gameObject, m_logProfile);
    }

    public virtual void ResetPage()
    {
        while (m_breadcrumb.Count > 0)
        {
            PageSection closing = m_breadcrumb.Pop();
            closing.ExitSection();
            closing.ResetSection();
        }
    }
    #endregion

    #region Section Navigation
    public virtual void EnterSection(PageSection section)
    {
        if (section == null)
            return;

        if (m_breadcrumb.Count > 0)
            m_breadcrumb.Peek().ExitSection();

        m_breadcrumb.Push(section);
        section.EnterSection();

        if (m_sectionsToReset.Contains(section))
            return;

        m_sectionsToReset.Add(section);
    }

    public virtual bool TryExitCurrentSection()
    {
        if (m_breadcrumb.Count == 0)
            return false;

        PageSection goBackFrom = m_breadcrumb.Pop();
        goBackFrom.ExitSection();

        if (m_breadcrumb.Count > 0)
        {
            PageSection landingSection = m_breadcrumb.Peek();
            landingSection.EnterSection();
            return true;
        }
       
        return false;
    }

    protected virtual void ResetSections()
    {
        foreach (PageSection section in m_sectionsToReset)
        {
            section.ResetSection();
        }

        m_sectionsToReset.Clear();
    }
    #endregion

    #region Controls 
    public virtual void Confirm()
    {
        if (m_breadcrumb.Count == 0 || m_breadcrumb.Peek() is not IHandleOnConfirm handler)
            return;

        handler.HandleOnConfirm();
    }

    public virtual void CycleUp()
    {
        if (m_breadcrumb.Count == 0 || m_breadcrumb.Peek() is not IHandleOnCycleUp handler)
            return;

        handler.HandleOnCycleUp();
    }

    public virtual void CycleDown()
    {
        if (m_breadcrumb.Count == 0 || m_breadcrumb.Peek() is not IHandleOnCycleDown handler)
            return;

        handler.HandleOnCycleDown();
    }

    public virtual void PageLeftLv1()
    {
        if (m_breadcrumb.Count == 0 || m_breadcrumb.Peek() is not IHandlePageLeftLv1 handler)
            return;

        handler.HandleOnPageLeftLv1();
    }

    public virtual void PageLeftLv2()
    {
        if (m_breadcrumb.Count == 0 || m_breadcrumb.Peek() is not IHandlePageLeftLv2 handler)
            return;

        handler.HandleOnPageLeftLv2();
    }

    public virtual void PageRightLv1()
    {
        if (m_breadcrumb.Count == 0 || m_breadcrumb.Peek() is not IHandlePageRightLv1 handler)
            return;

        handler.HandleOnPageRightLv1();
    }

    public virtual void PageRightLv2()
    {
        if (m_breadcrumb.Count == 0 || m_breadcrumb.Peek() is not IHandlePageRightLv2 handler)
            return;

        handler.HandleOnPageRightLv2();
    }

    #endregion
}
