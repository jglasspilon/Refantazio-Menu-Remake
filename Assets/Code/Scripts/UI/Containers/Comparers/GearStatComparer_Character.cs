using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GearStatComparer_Character : MonoBehaviour
{
    [SerializeField] private EItemCategories m_equipmentType;
    [SerializeField] private EStatType m_statToCheck;
    [SerializeField] private bool m_displaySelfResult;
    [SerializeField] private MaskableGraphic[] m_graphicsToColor;
    [SerializeField] private GameObject[] m_upContent, m_downContent, m_noChangeContent;
    [SerializeField] private Color m_upColor, m_downColor, m_noChangeColor;
    [Header("Text Settings:")]
    [SerializeField] private TextMeshProUGUI m_resultText;
    [SerializeField] private StringFormatter[] m_formatters;

    private Character m_character, m_simulatedCharacter;

    public EItemCategories EquipmentType => m_equipmentType;

    private void OnDisable()
    {
        m_character = null;
    }

    public void InitializeWithCharacter(Character character)
    {
        m_character = character;
    }

    public void HandleOnItemChange(InventoryEntry entry)
    {
        if (entry.Item is Equipment equipment)
        {
            m_simulatedCharacter = m_character.CreateSimulatedCharacter();
            m_simulatedCharacter.Equipment.EquipFromType(equipment.Category, equipment);
            CompareAndShow(equipment);
            return;
        }

        ShowNoChange();
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
        DisplayResultText();
    }

    private void ShowNoChange()
    {
        m_graphicsToColor.ForEach(x => x.color = m_noChangeColor);
        m_upContent.ForEach(x => x.SetActive(false));
        m_downContent.ForEach(x => x.SetActive(false));
        m_noChangeContent.ForEach(x => x.SetActive(true));
        DisplayResultText(true);
    }

    private void DisplayResultText(bool showUnChanged = false)
    {
        Character characterToPullStatFrom = m_displaySelfResult && !showUnChanged ? m_simulatedCharacter : m_character;
        string result = characterToPullStatFrom.Stats.GetStat(m_statToCheck).Final.Value.ToString();

        foreach(StringFormatter formatter in m_formatters)
        {
            result = formatter.Format(result, out string message);
        }

        m_resultText.text = result;
    }
}
