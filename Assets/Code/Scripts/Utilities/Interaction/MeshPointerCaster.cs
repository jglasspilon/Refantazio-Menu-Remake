using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Camera))]
[RequireComponent(typeof(PhysicsRaycaster))]
public class MeshPointerCaster : MonoBehaviour
{
    [SerializeField]
    private LoggingProfile m_logProfile;

    [SerializeField]
    private bool m_debugLine;

    private Camera m_cam;
    private GameObject m_itemEntered;
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
            if(m_debugLine)
                Debug.DrawLine(m_cam.transform.position, hit.point, Color.red, 0f, false);

            if (m_itemEntered == null)
            {
                OnEnter(hit);
            }
            else if (m_itemEntered != hit.collider.gameObject)
            {
                OnEnter(hit);
                OnExit(hit);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnClick(hit);
            }

            m_itemEntered = hit.collider.gameObject;
            m_lastHit = hit;
        }
        else if (m_itemEntered != null)
        {
            OnExit(m_lastHit);
            m_itemEntered = null;
        }
    }

    private void OnEnter(RaycastHit hit)
    {
        string collisionName = GetCollisionName(hit.collider.gameObject);
        Logger.Log($"Entering {collisionName}", gameObject, m_logProfile);
        IPointerEnterHandler hitEnterHandler = hit.collider.gameObject.GetComponent<IPointerEnterHandler>();
        hitEnterHandler?.OnPointerEnter(Helper.Interaction.PointerEventFactory.FromRaycastHit(m_eventSystem, m_cam, hit, m_raycaster));
    }

    private void OnExit(RaycastHit hit)
    {
        string collisionName = GetCollisionName(m_itemEntered.gameObject);
        Logger.Log($"Exitting {collisionName}", gameObject, m_logProfile);
        IPointerExitHandler hitEnterHandler = m_itemEntered.gameObject.GetComponent<IPointerExitHandler>();
        hitEnterHandler?.OnPointerExit(Helper.Interaction.PointerEventFactory.FromRaycastHit(m_eventSystem, m_cam, hit, m_raycaster));
    }

    private void OnClick(RaycastHit hit)
    {
        string collisionName = GetCollisionName(hit.collider.gameObject);
        Logger.Log($"Clicking {collisionName}", gameObject, m_logProfile);
        IPointerClickHandler hitEnterHandler = hit.collider.gameObject.GetComponent<IPointerClickHandler>();
        hitEnterHandler?.OnPointerClick(Helper.Interaction.PointerEventFactory.FromRaycastHit(m_eventSystem, m_cam, hit, m_raycaster));
    }

    private Vector2 MapMouseToRenderTexture()
    {
        float x = Input.mousePosition.x * (m_renderTexture.width / (float)Screen.width);
        float y = Input.mousePosition.y * (m_renderTexture.height / (float)Screen.height);
        return new Vector2(x, y);
    }

    private string GetCollisionName(GameObject collisionObject)
    {
        if (collisionObject == null)
            return "empty";

        if (collisionObject.TryGetComponent(out PointerEventForwarder forwarder))
            return forwarder.Target.name;

        return collisionObject.name;
    }
}
