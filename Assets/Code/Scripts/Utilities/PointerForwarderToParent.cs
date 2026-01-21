using UnityEngine;
using UnityEngine.EventSystems;

public class PointerForwarderToParent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private GameObject m_target;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerEnterHandler>(
            m_target, eventData, (h, d) => h.OnPointerEnter(d as PointerEventData));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerExitHandler>(
            m_target, eventData, (h, d) => h.OnPointerExit(d as PointerEventData));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerClickHandler>(
            m_target, eventData, (h, d) => h.OnPointerClick(d as PointerEventData));
    }
}

