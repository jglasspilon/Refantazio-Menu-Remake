using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Camera))]
[RequireComponent(typeof(PhysicsRaycaster))]
public class MeshIPointerCaster : MonoBehaviour
{
    private Camera m_cam;
    private GameObject m_hit;
    private RenderTexture m_renderTexture;
    private PhysicsRaycaster m_raycaster;
    private EventSystem m_eventSystem;
    private RaycastHit m_lastHit;

    private void Awake()
    {
        m_cam = GetComponent<Camera>();
        m_renderTexture = m_cam.targetTexture;
        m_eventSystem = FindFirstObjectByType<EventSystem>();
        m_raycaster = GetComponent<PhysicsRaycaster>();
    }

    private void Update()
    {
        Vector2 mousePos = m_renderTexture != null ? MapMouseToRenderTexture() : Input.mousePosition;

        if (Physics.Raycast(m_cam.ScreenPointToRay(mousePos), out RaycastHit hit))
        {
            Debug.DrawLine(m_cam.transform.position, hit.point, Color.red, 0f, false);
            if (m_hit == null)
            {
                OnEnter(hit);
            }
            else if (m_hit != hit.collider.gameObject)
            {
                OnEnter(hit);
                OnExit(hit);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnClick(hit);
            }

            m_hit = hit.collider.gameObject;
            m_lastHit = hit;
        }
        else if (m_hit != null)
        {
            OnExit(m_lastHit);
            m_hit = null;
        }
    }

    private void OnEnter(RaycastHit hit)
    {
        IPointerEnterHandler hitEnterHandler = hit.collider.gameObject.GetComponent<IPointerEnterHandler>();
        hitEnterHandler?.OnPointerEnter(Helper.PointerEventFactory.FromRaycastHit(m_eventSystem, m_cam, hit, m_raycaster));
    }

    private void OnExit(RaycastHit hit)
    {
        IPointerExitHandler hitEnterHandler = m_hit.gameObject.GetComponent<IPointerExitHandler>();
        hitEnterHandler?.OnPointerExit(Helper.PointerEventFactory.FromRaycastHit(m_eventSystem, m_cam, hit, m_raycaster));
    }

    private void OnClick(RaycastHit hit)
    {
        IPointerClickHandler hitEnterHandler = hit.collider.gameObject.GetComponent<IPointerClickHandler>();
        hitEnterHandler?.OnPointerClick(Helper.PointerEventFactory.FromRaycastHit(m_eventSystem, m_cam, hit, m_raycaster));
    }

    private Vector2 MapMouseToRenderTexture()
    {
        float x = Input.mousePosition.x * (m_renderTexture.width / (float)Screen.width);
        float y = Input.mousePosition.y * (m_renderTexture.height / (float)Screen.height);
        return new Vector2(x, y);
    }
}
