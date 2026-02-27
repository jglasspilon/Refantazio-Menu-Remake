using UnityEngine;

public class ShowOnSelected : MonoBehaviour
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

        m_parentSelectable.OnSetAsSelected += HandleOnSetAsSelected;
    }

    private void OnDestroy()
    {
        if (m_parentSelectable == null)
            return;

        m_parentSelectable.OnSetAsSelected -= HandleOnSetAsSelected;
    }

    private void HandleOnSetAsSelected(bool selected)
    {
        if (m_invert)
            selected = !selected;

        gameObject.SetActive(selected);
    }
}
