using UnityEngine;

public class DisplayOnNewItem : MonoBehaviour, IBindableToInventoryEntry
{
    private InventoryEntry m_entry;

    public void BindToInventoryEntry(InventoryEntry entry)
    {
        if (entry == null)
            return;

        m_entry = entry;
        m_entry.OnMarkAsSeen += Display;
        Display(m_entry);
    }

    public void Unbind()
    {
        if (m_entry != null)
            return;

        m_entry.OnMarkAsSeen -= Display;
        m_entry = null;
    }

    public void Display(InventoryEntry enrty)
    {
        gameObject.SetActive(enrty.IsNew);
    }
}
