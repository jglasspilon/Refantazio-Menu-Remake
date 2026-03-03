using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayEquipmentStat : MonoBehaviour, IBindableToInventoryEntry
{
    [SerializeField]
    private EEquipeModifierType m_targetEquipmentStat;

    [SerializeField]
    private TextMeshProUGUI m_statText, m_statValueText;

    private InventoryEntry m_entry;
    private Dictionary<EEquipeModifierType, int> m_textSizes = new Dictionary<EEquipeModifierType, int>()
    {
        {EEquipeModifierType.Main, 3 },
        {EEquipeModifierType.Secondary, 2 }
    };

    private enum EEquipeModifierType
    {
        Main,
        Secondary
    }

    public void BindToInventoryEntry(InventoryEntry entry)
    {
        if (entry == null || entry.Item is not Equipment equip)
        {
            Clear();
            return;
        }

        m_entry = entry;
        StatModifier statModifider = GetStatFromEquipment(equip, m_targetEquipmentStat);
        Display(statModifider.Type, statModifider.Amount);
    }

    public void Unbind()
    {
        m_entry = null;
    }

    private void Display(EStatType statType, int value)
    {
        m_statText.text = Helper.StringFormatting.PrettifyStat(statType);
        m_statValueText.text = Helper.StringFormatting.FormatIntForUI(value, m_textSizes[m_targetEquipmentStat], m_statValueText.color.g > 0.5f);
    }    

    private void Clear()
    {
        m_statText.text = "";
        m_statValueText.text = "";
    }

    private StatModifier GetStatFromEquipment(Equipment equip, EEquipeModifierType modifierType)
    {
        if (modifierType == EEquipeModifierType.Main)
            return equip.MainModifier;

        return equip.SecondaryModifier;
    }
}
