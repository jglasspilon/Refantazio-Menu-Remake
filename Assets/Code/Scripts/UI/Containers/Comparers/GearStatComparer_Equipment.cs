using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearStatComparer_Equipment : MonoBehaviour
{
    [SerializeField] private bool m_useMainModifier, m_displaySelfResult;
    [SerializeField] private MaskableGraphic[] m_graphicsToColor;
    [SerializeField] private GameObject[] m_upContent, m_downContent, m_noChangeContent;
    [SerializeField] private Color m_upColor, m_downColor, m_noChangeColor;

    private Equipment m_equipment;
    private Character m_character, m_simulatedCharacter;
    private EStatType m_statToCheck;

    private void OnDisable()
    {
        m_character = null;
    }

    public void InitializeWithCharacter(Character character)
    {
        if (character == null || m_equipment == null) 
        {
            ShowNoChange();
            return;
        }

        m_character = character;
        m_statToCheck = m_useMainModifier ? m_equipment.MainModifier.Type : m_equipment.SecondaryModifier.Type;
        m_simulatedCharacter = m_character.CreateSimulatedCharacter();
        m_simulatedCharacter.Equipment.EquipFromType(m_equipment.Category, m_equipment);
        CompareAndShow(m_equipment);  
    }

    public void InitializeWithItem(InventoryEntry entry)
    {
        if (entry.Item is Equipment equipment)
        {
            m_equipment = equipment;          
        }      
    }

    public void CompareAndShow(Equipment equipment)
    {
        if (m_character == null || m_simulatedCharacter == null)
        {
            ShowNoChange();
            return;
        }

        Stat selfStat = m_simulatedCharacter.Stats.GetStat(m_statToCheck);
        Stat compareStat = m_character.Stats.GetStat(m_statToCheck);

        bool up = selfStat.Final.Value > compareStat.Final.Value;
        bool down = selfStat.Final.Value < compareStat.Final.Value;
        bool noChange = !up && !down;

        //Invert result if showing the result relative to the compared Equipment instead of the registered equipment
        if(!m_displaySelfResult && !noChange)
        {
            up = !up;
            down = !down;
        }

        Color colorToUse = up ? m_upColor : down ? m_downColor : m_noChangeColor;

        m_graphicsToColor.ForEach(x => x.color = colorToUse);
        m_upContent.ForEach(x => x.SetActive(up));
        m_downContent.ForEach(x => x.SetActive(down));
        m_noChangeContent.ForEach(x => x.SetActive(noChange));
    }

    private void ShowNoChange()
    {
        m_graphicsToColor.ForEach(x => x.color = m_noChangeColor);
        m_upContent.ForEach(x => x.SetActive(false));
        m_downContent.ForEach(x => x.SetActive(false));
        m_noChangeContent.ForEach(x => x.SetActive(true));
    }
}
