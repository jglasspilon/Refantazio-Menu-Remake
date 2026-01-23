using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEventForwarder : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    private GameObject m_target;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerEnterHandler>(m_target, eventData, (t, d) => t.OnPointerEnter(d as PointerEventData));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerExitHandler>(m_target, eventData, (t, d) => t.OnPointerExit(d as PointerEventData));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerClickHandler>(m_target, eventData, (t, d) => t.OnPointerClick(d as PointerEventData));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerDownHandler>(m_target, eventData, (t, d) => t.OnPointerDown(d as PointerEventData));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerUpHandler>(m_target, eventData, (t, d) => t.OnPointerUp(d as PointerEventData));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IBeginDragHandler>(m_target, eventData, (t, d) => t.OnBeginDrag(d as PointerEventData));
    }

    public void OnDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IDragHandler>(m_target, eventData, (t, d) => t.OnDrag(d as PointerEventData));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IEndDragHandler>(m_target, eventData, (t, d) => t.OnEndDrag(d as PointerEventData));
    }

    public void OnDrop(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IDropHandler>(m_target, eventData, (t, d) => t.OnDrop(d as PointerEventData));
    }
}

