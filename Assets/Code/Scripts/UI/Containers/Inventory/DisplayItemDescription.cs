using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayItemDescription : MonoBehaviour, IBindableToInventoryEntry
{
    [SerializeField]
    private Color m_healDescriptionColor, m_damageDescriptionColor;

    private TextMeshProUGUI m_text;
    private InventoryEntry m_entry;

    private void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
    }

    public void BindToInventoryEntry(InventoryEntry entry)
    {
        if (entry == null || entry.Item == null)
            return;

        m_entry = entry;
        Display(m_entry.Item.Description);
    }

    public void Unbind()
    {
        m_entry = null;
    }

    private void Display(string description)
    {
        if (!description.Contains("<m>"))
        {
            m_text.text = description;
            return;
        }

        if(m_entry.Item is UsableItem usable)
        {
            string usableColorTag = usable.TargetingType.ToString().ToLower().Contains("foe") ? ColorUtility.ToHtmlStringRGB(m_damageDescriptionColor) : ColorUtility.ToHtmlStringRGB(m_healDescriptionColor);
            m_text.text = description.Replace("<m>", $"<color=#{usableColorTag}>");
            return;
        }

        string colorTag = ColorUtility.ToHtmlStringRGB(m_healDescriptionColor);
        m_text.text = description.Replace("<m>", $"<color=#{colorTag}>");
    }
}
