using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class EquipmentSortingDisplay : MonoBehaviour
{
    [SerializeField] private EquipmentSlotSelecter m_slotSelecter;

    private TextMeshProUGUI m_text;

    private void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        HandleSortChange(0);
    }

    public void HandleSortChange(EEquipmentSortType sortType)
    {
        if (m_slotSelecter.SelectedObject == null || m_slotSelecter.SelectedObject.SlotContent is not Equipment equipment)
            return;

        if(sortType == EEquipmentSortType.SecondaryStat)
            m_text.text = Helper.StringFormatting.PrettifyStat(equipment.SecondaryModifier.Type);
        else
            m_text.text = Helper.StringFormatting.PrettifyStat(equipment.MainModifier.Type);
    }
}
