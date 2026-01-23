using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MenuPage : MonoBehaviour
{
    public event Action OnOpened, OnClosed;

    [SerializeField]
    private EMenuPages m_pageName;

    [SerializeField]
    private Animator m_anim;

    private Stack<object> m_breadcrumb = new Stack<object>(); //TODO: change to actual page segment once implemented. 
    public EMenuPages PageName {  get { return m_pageName; } }

    public virtual async UniTask OpenAsync(int pageCount)
    {
        gameObject.SetActive(true);
        m_anim.SetInteger("PageCount", pageCount);
        m_anim.SetBool("IsActive", true);
        await Helper.Animation.WaitForCurrentPageAnimationToEnd(m_anim);
    }

    public virtual async UniTask CloseAsync()
    {
        m_anim.SetBool("IsActive", false);
        await Helper.Animation.WaitForCurrentPageAnimationToEnd(m_anim);
        gameObject.SetActive(false);
    }

    public virtual bool TryGoBack()
    {
        if(m_breadcrumb.Count > 0)
        {
            object goBackFrom = m_breadcrumb.Pop(); //TODO: change to actual page segment once implemented
            //TODO: close last breadcrumb 
            return true;
        }

        return false;
    }

    public virtual void Close()
    {
        m_anim.SetBool("IsActive", false);
        gameObject.SetActive(false);
    }

    public abstract void CycleUp();
    public abstract void CycleDown();
    public abstract void Confirm();
    public abstract void ResetPage();
}
