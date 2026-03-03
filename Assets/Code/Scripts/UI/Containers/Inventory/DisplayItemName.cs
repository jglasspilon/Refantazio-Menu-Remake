using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayItemName : MonoBehaviour, IBindableToInventoryEntry
{
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
        Display(m_entry.Item.Name);

    }

    public void Unbind()
    {
        m_entry = null;
    }

    private void Display(string name)
    {
        m_text.text = name;
    }
}
