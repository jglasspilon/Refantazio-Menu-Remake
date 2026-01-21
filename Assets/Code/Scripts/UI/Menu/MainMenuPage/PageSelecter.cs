using UnityEngine;
using UnityEngine.EventSystems;

public class PageSelecter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private EMenuPages m_pageName;

    private MainMenuPage m_parentPage;

    private void Awake()
    {
        m_parentPage = GetComponentInParent<MainMenuPage>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Entered {m_pageName}");   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Exited {m_pageName}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked {m_pageName}");
    }
}
