using TMPro;
using UnityEngine;

public class DisplayEquipmentEffect : MonoBehaviour, IBindableToInventoryEntry
{
    [SerializeField]
    private TextMeshProUGUI m_text;

    private InventoryEntry m_entry;

    public void BindToInventoryEntry(InventoryEntry entry)
    {
        if (entry == null || entry.Item is not Equipment equip)
        {
            Display(null);
            return;
        }

        m_entry = entry;
        Display(equip.Effect);
    }

    public void Unbind()
    {
        m_entry = null;
    }

    private void Display(EquipEffect effect)
    {
        gameObject.SetActive(effect != null);

        if (effect == null)
            return;

        m_text.text = effect.Description;
    }
}
