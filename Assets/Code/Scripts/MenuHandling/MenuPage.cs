using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
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
        await WaitForCurrentPageAnimationToEnd();
    }

    public virtual async UniTask CloseAsync()
    {
        m_anim.SetBool("IsActive", false);
        await WaitForCurrentPageAnimationToEnd();
        gameObject.SetActive(false);
    }

    public virtual void Confirm()
    {
        //TODO: show new segment
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

    public abstract void ResetPage();

    protected async UniTask WaitForCurrentPageAnimationToEnd()
    {
        await UniTask.NextFrame();
        await UniTask.Delay(TimeSpan.FromSeconds(m_anim.GetCurrentAnimatorStateInfo(0).length), ignoreTimeScale: false);
    }
}
