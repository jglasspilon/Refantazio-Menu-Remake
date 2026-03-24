using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class PageSection: MonoBehaviour, IHandleOnConfirm, IHandleOnExtraOption, IHandleOnBack, IHandleOnCycleDown, IHandleOnCycleUp, IHandlePageLeftLv1, 
    IHandlePageLeftLv2, IHandlePageRightLv1, IHandlePageRightLv2
{
    [Header("Life Cycle Events:")]
    [Space]
    [SerializeField] protected UnityEvent m_onEnter;
    [SerializeField] protected UnityEvent m_onExit;

    [Header("Input Events:")]
    [Space]
    public UnityEvent OnBack;
    public UnityEvent OnConfirm, OnExtraOption, OnCycleUp, OnCycleDown, OnPageLeftLv1, OnPageLeftLv2, OnPageRightLv1, OnPageRightLv2;
    public abstract UniTask EnterSection();
    public abstract UniTask ExitSection();

    public virtual void HandleOnBack()
    {
        OnBack?.Invoke();
    }

    public virtual void HandleOnConfirm()
    {
        OnConfirm?.Invoke();
    }

    public void HandleOnExtraOption()
    {
        OnExtraOption?.Invoke();
    }

    public virtual void HandleOnCycleDown()
    {
        OnCycleDown?.Invoke();
    }

    public virtual void HandleOnCycleUp()
    {
        OnCycleUp?.Invoke();
    }

    public virtual void HandleOnPageLeftLv1()
    {
        OnPageLeftLv1?.Invoke();
    }

    public virtual void HandleOnPageLeftLv2()
    {
        OnPageLeftLv2?.Invoke();
    }

    public virtual void HandleOnPageRightLv1()
    {
        OnPageRightLv1?.Invoke();
    }

    public virtual void HandleOnPageRightLv2()
    {
        OnPageRightLv2?.Invoke();
    }

    public abstract void ResetSection();
}
