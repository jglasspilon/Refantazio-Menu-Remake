using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private HUDWidget[] m_widgets;

    private void Awake()
    {
        m_widgets = GetComponentsInChildren<HUDWidget>();
    }

    public async UniTask OpenAsync()
    {
        
    }

    public async UniTask CloseAsync()
    {
        
    }
}
