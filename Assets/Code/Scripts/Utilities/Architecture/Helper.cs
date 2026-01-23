using UnityEngine;
using UnityEngine.EventSystems;

public class Helper
{
    public static class PointerEventFactory
    {
        public static PointerEventData FromRaycastHit(EventSystem eventSystem, Camera cam, RaycastHit hit, BaseRaycaster raycaster)
        {
            var data = new PointerEventData(eventSystem);

            // Convert world hit point → screen position
            data.position = cam.WorldToScreenPoint(hit.point);

            // Build a RaycastResult that mimics a UI hit
            data.pointerCurrentRaycast = new RaycastResult
            {
                worldPosition = hit.point,
                worldNormal = hit.normal,
                screenPosition = data.position,
                module = raycaster,
                gameObject = hit.collider.gameObject,
                distance = hit.distance
            };

            return data;
        }
    }

}
