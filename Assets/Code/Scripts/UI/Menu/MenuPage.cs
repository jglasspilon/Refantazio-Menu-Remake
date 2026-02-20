using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MenuPage : MonoBehaviour
{
    public event Action OnOpening, OnOpened, OnClosing, OnClosed;

    [SerializeField]
    private EMenuPages m_pageName;

    [SerializeField]
    private Animator m_anim;

    [SerializeField]
    protected LoggingProfile m_logProfile;

    protected Stack<PageSection> m_breadcrumb = new Stack<PageSection>(); 
    public EMenuPages PageName => m_pageName;
    public int PageCount { get; private set; }
    protected virtual PageSection CurrentPageSection => m_breadcrumb.Peek();

    public virtual async UniTask OpenAsync(int pageCount)
    {
        OnOpening?.Invoke();

        Logger.Log($"Opening menu page '{PageName}'.", gameObject, m_logProfile);
        gameObject.SetActive(true);
        PageCount = pageCount;

        await EnterDefaultSection();
        m_anim.SetInteger("PageCount", pageCount);
        m_anim.SetBool("IsActive", true);

        await Helper.Animation.WaitForCurrentPageAnimationToEnd(m_anim);

        Logger.Log($"Menu page '{PageName}' opened successfully.", gameObject, m_logProfile);
        OnOpened?.Invoke();
    }

    public virtual UniTask EnterDefaultSection()
    {
        return default;
    }

    public virtual async UniTask CloseAsync()
    {
        OnClosing?.Invoke();
        Logger.Log($"Closing menu page '{PageName}'.", gameObject, m_logProfile);        
        m_anim.SetBool("IsActive", false);
        await Helper.Animation.WaitForCurrentPageAnimationToEnd(m_anim);

        gameObject.SetActive(false);
        Logger.Log($"Menu page '{PageName}' closed successfully.", gameObject, m_logProfile);
        OnClosed?.Invoke();
    }

    public virtual void EnterSection(PageSection section)
    {
        m_breadcrumb.Push(section);
        CurrentPageSection.ExitSection();
        section.EnterSection();
    }

    public virtual bool TryGoBack()
    {
        if(m_breadcrumb.Count > 0)
        {
            PageSection goBackFrom = m_breadcrumb.Pop();
            PageSection landingSection = m_breadcrumb.Peek();

            goBackFrom.ExitSection();
            landingSection.EnterSection();
            return true;
        }

        return false;
    }

    public virtual void Close()
    {
        if(m_anim.isActiveAndEnabled)
            m_anim.SetBool("IsActive", false);

        gameObject.SetActive(false);
        Logger.Log($"Menu page {PageName} closed successfully.", gameObject, m_logProfile);
    }

    public virtual void Confirm()
    {
        if (CurrentPageSection is IHandleOnConfirm handler)
            handler.OnConfirm();
    }

    public virtual void CycleUp()
    {
        if (CurrentPageSection is IHandleOnCycleUp handler)
            handler.OnCycleUp();
    }

    public virtual void CycleDown()
    {
        if (CurrentPageSection is IHandleOnCycleDown handler)
            handler.OnCycleDown();
    }

    public abstract void ResetPage();
}
