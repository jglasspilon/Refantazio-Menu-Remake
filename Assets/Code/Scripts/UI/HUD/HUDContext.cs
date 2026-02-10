using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class HUDContext : MonoBehaviour
{
    [SerializeField]
    private EGameState m_hudName; 

    [SerializeField]
    private LoggingProfile m_logProfile;

    private Dictionary<EWidgetTypes, HUDWidget> m_widgets = new Dictionary<EWidgetTypes, HUDWidget>();

    public EGameState HUDName { get { return m_hudName; } }

    private void Awake()
    {
        m_widgets = GetComponentsInChildren<HUDWidget>().ToDictionary(x => x.WidgetType);
    }

    public async UniTask OpenAsync()
    {
        Logger.Log($"Opening HUD '{HUDName}'.", gameObject, m_logProfile);
        gameObject.SetActive(true);
        await UniTask.WhenAll(m_widgets.Values.Select(x => x.ShowAsync()));
        Logger.Log($"HUD '{HUDName}' opened successfully.", gameObject, m_logProfile);
    }

    public async UniTask CloseAsync()
    {
        Logger.Log($"Closing HUD '{HUDName}'.", gameObject, m_logProfile);
        await UniTask.WhenAll(m_widgets.Values.Select(x => x.HideAsync()));
        gameObject.SetActive(false);
        Logger.Log($"HUD '{HUDName}' closed successfully.", gameObject, m_logProfile);
    }

    public void ShowWidgets(EWidgetTypes[] widgetsToShow)
    {
        foreach (EWidgetTypes widgetType in widgetsToShow)
        {
            if(m_widgets.TryGetValue(widgetType, out HUDWidget widget))
            {
                widget.ShowAsync();
            }
        }
    }

    public void HideWidgets(EWidgetTypes[] widgetsToHide, bool hideInstant = false)
    {
        foreach (EWidgetTypes widgetType in widgetsToHide)
        {
            if (m_widgets.TryGetValue(widgetType, out HUDWidget widget))
            {
                if (hideInstant)
                    widget.Hide();
                else
                    widget.HideAsync();
            }
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        foreach (HUDWidget widget in m_widgets.Values)
        {
            widget.Hide();
        }
        Logger.Log($"HUD '{HUDName}' closed successfully.", gameObject, m_logProfile);
    }
}


