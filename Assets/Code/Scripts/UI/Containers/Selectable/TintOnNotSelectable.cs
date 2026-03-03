using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class TintOnNotSelectable : MonoBehaviour
{
    [SerializeField]
    private Color m_tint;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private MaskableGraphic m_graphic;
    private ISelectable m_parentSelectable;
    private Color m_originalColor;

    private void Awake()
    {
        m_graphic = GetComponent<MaskableGraphic>();
        m_parentSelectable = GetComponentInParent<ISelectable>();
        m_originalColor = m_graphic.color;

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

    private void HandleOnSetAsSelectable(bool selectable)
    {
        m_graphic.color = selectable ? m_originalColor : m_tint;
    }
}
