using UnityEngine;

public class ShowOnSelectable : MonoBehaviour
{
    [SerializeField]
    private bool m_invert;
    
    [SerializeField]
    private LoggingProfile m_logProfile;
    
    private ISelectable m_parentSelectable;

    private void Awake()
    {
        m_parentSelectable = GetComponentInParent<ISelectable>();

        if (m_parentSelectable == null)
        {
            Logger.LogError($"{gameObject.name} ShowOnSelect component failed to find a ISelectable parent.", m_logProfile);
            return;
        }

        m_parentSelectable.OnSetAsSelectable += HandleOnSetAsSelectable;
    }

    private void OnDestroy()
    {
        if (m_parentSelectable == null)
            return;

        m_parentSelectable.OnSetAsSelectable -= HandleOnSetAsSelectable;
    }

    private void HandleOnSetAsSelectable(bool selected)
    {
        if (m_invert)
            selected = !selected;

        gameObject.SetActive(selected);
    }
}
