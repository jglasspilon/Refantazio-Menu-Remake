using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DisplayItemIcon : MonoBehaviour, IBindableToInventoryEntry
{
    private Image m_icon;
    private InventoryEntry m_entry;

    private void Awake()
    {
        m_icon = GetComponent<Image>();    
    }

    public void BindToInventoryEntry(InventoryEntry entry)
    {
        if (entry == null || entry.Item == null)
            return;

        m_entry = entry;
        Display(m_entry.Item.Icon);
    }

    public void Unbind()
    {
        m_entry = null;
    }

    private void Display(Sprite icon)
    {
        m_icon.enabled = icon != null;
        m_icon.sprite = icon;
    }
}
