using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayItemCount : MonoBehaviour, IBindableToInventoryEntry
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
        m_entry.OnAmountChanged += Display;
        Display(m_entry.Count);
    }

    public void Unbind()
    {
        if (m_entry != null)
            return;

        m_entry.OnAmountChanged -= Display;
        m_entry = null;
    }

    private void Display(int amount)
    {
        m_text.text = Helper.StringFormatting.FormatIntForUI(amount, 2, true);
    }
}
