using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class PageSection: MonoBehaviour, IHandleOnConfirm, IHandleOnBack, IHandleOnCycleDown, IHandleOnCycleUp, IHandlePageLeftLv1, 
    IHandlePageLeftLv2, IHandlePageRightLv1, IHandlePageRightLv2
{
    public event Action OnBack, OnConfirm, OnCycleUp, OnCycleDown, OnPageLeftLv1, OnPageLeftLv2, OnPageRightLv1, OnPageRightLv2;
    public abstract UniTask EnterSection();
    public abstract UniTask ExitSection();

    public void HandleOnBack()
    {
        OnBack?.Invoke();
    }

    public void HandleOnConfirm()
    {
        OnConfirm?.Invoke();
    }

    public void HandleOnCycleDown()
    {
        OnCycleDown?.Invoke();
    }

    public void HandleOnCycleUp()
    {
        OnCycleUp?.Invoke();
    }

    public void HandleOnPageLeftLv1()
    {
        OnPageLeftLv1?.Invoke();
    }

    public void HandleOnPageLeftLv2()
    {
        OnPageLeftLv2?.Invoke();
    }

    public void HandleOnPageRightLv1()
    {
        OnPageRightLv1?.Invoke();
    }

    public void HandleOnPageRightLv2()
    {
        OnPageRightLv2?.Invoke();
    }

    public abstract void ResetSection();
}
