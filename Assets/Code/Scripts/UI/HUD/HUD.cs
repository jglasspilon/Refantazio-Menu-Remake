using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private EGameState m_hudName; 

    [SerializeField]
    private Animator m_anim;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private HUDWidget[] m_widgets;

    public EGameState HUDName { get { return m_hudName; } }

    private void Awake()
    {
        m_widgets = GetComponentsInChildren<HUDWidget>();
    }

    public async UniTask OpenAsync()
    {
        Logger.Log($"Opening HUD '{HUDName}'.", gameObject, m_logProfile);
        gameObject.SetActive(true);
        m_anim.SetBool("IsActive", true);
        await Helper.Animation.WaitForCurrentPageAnimationToEnd(m_anim);

        Logger.Log($"HUD '{HUDName}' opened successfully.", gameObject, m_logProfile);
    }

    public async UniTask CloseAsync()
    {
        Logger.Log($"Closing HUD '{HUDName}'.", gameObject, m_logProfile);
        m_anim.SetBool("IsActive", false);
        await Helper.Animation.WaitForCurrentPageAnimationToEnd(m_anim);

        gameObject.SetActive(false);
        Logger.Log($"HUD '{HUDName}' closed successfully.", gameObject, m_logProfile);
    }

    public void Close()
    {
        m_anim.SetBool("IsActive", false);
        gameObject.SetActive(false);
        Logger.Log($"HUD '{HUDName}' closed successfully.", gameObject, m_logProfile);
    }
}
