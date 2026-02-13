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
    private LoggingProfile m_logProfile;

    private Stack<IPageSection> m_breadcrumb = new Stack<IPageSection>(); 
    public EMenuPages PageName => m_pageName;
    public int PageCount { get; private set; }

    public virtual async UniTask OpenAsync(int pageCount)
    {
        OnOpening?.Invoke();
        Logger.Log($"Opening menu page '{PageName}'.", gameObject, m_logProfile);
        gameObject.SetActive(true);
        PageCount = pageCount;
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

        gameObject.SetActive(false);
        Logger.Log($"Menu page '{PageName}' closed successfully.", gameObject, m_logProfile);
        OnClosed?.Invoke();
    }

    public virtual void EnterSection(IPageSection section)
    {
        m_breadcrumb.Push(section);
        section.Enter();
    }

    public virtual bool TryGoBack()
    {
        if(m_breadcrumb.Count > 0)
        {
            IPageSection goBackFrom = m_breadcrumb.Pop();
            goBackFrom.Exit();
            return true;
        }

        return false;
    }

    public virtual void Close()
    {
        m_anim.SetBool("IsActive", false);
        gameObject.SetActive(false);
        Logger.Log($"Menu page {PageName} closed successfully.", gameObject, m_logProfile);
    }

    public abstract void CycleUp();
    public abstract void CycleDown();
    public abstract void Confirm();
    public abstract void ResetPage();
}
