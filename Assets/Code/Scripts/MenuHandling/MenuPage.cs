using System;
using System.Collections;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    public event Action OnOpened, OnClosed;

    [SerializeField]
    private EMenuPages m_pageName;

    [SerializeField]
    private Animator m_anim;

    public EMenuPages PageName {  get { return m_pageName; } }

    public virtual void Open(int pageCount)
    {
        m_anim.SetInteger("PageCount", pageCount);
        m_anim.SetBool("IsActive", true);       
    }

    public virtual void Close()
    {
        m_anim.SetBool("IsActive", false);
    }

    /// <summary>
    /// Animation event function for invoking the OnOpened event. Should not be used by external classes.
    /// </summary>
    public void AnimEvent_OpenFinished()
    {
        OnOpened?.Invoke();
    }

    /// <summary>
    /// Animation event function for invoking the OnClosed event. Should not be used by external classes.
    /// </summary>
    public void AnimEvent_CloseFinished()
    {
        OnClosed?.Invoke();
    }
}
