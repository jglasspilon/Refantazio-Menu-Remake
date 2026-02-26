using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem_Equipment : InventoryItemUI
{
    [SerializeField]
    private Image m_icon;

    [SerializeField]
    private TextMeshProUGUI m_nameText, m_countText, m_descriptionText, m_effectDescription;

    [SerializeField]
    private TextMeshProUGUI[] m_mainTexts, m_mainTextValues, m_secondaryTexts, m_secondaryTextValues; 

    [SerializeField]
    private CanvasGroup m_alphaGroup;

    [SerializeField]
    private GameObject m_newFlag, m_selectionSplotch, m_selectionFrame, m_shadow, m_effectContent;

    [SerializeField]
    private Animator m_anim;

    [SerializeField]
    private Color m_markupDescriptionColor;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private InventoryEntry m_inventoryEntry;
    private const float UNUSABLE_ALPHA = 0.5f;
    private const float USABLE_ALPHA = 1.0f;

    public override InventoryEntry InventoryEntry => m_inventoryEntry;

    public override void InitializeFromData(InventoryEntry entry)
    {
        if(entry == null || entry.Item == null)
        {
            Logger.LogError("Received an empty item. Initializing an empty item is not allowed.", m_logProfile);
            return;
        }

        if(entry.Item is not Equipment weapon)
        {
            Logger.LogError("Type mismatch. Initializing a weapon from any other item type is not allowed.", m_logProfile);
            return;
        }

        transform.localScale = Vector3.one;
        m_inventoryEntry = entry;
        DisplayItemData(entry.Item);
        DisplayCount(entry.Count);
        DisplayAsNew(entry);
        DisplayStatsData(weapon);
        DisplayEffectsData(weapon);
        SetAsSelectable(entry.Item is not UsableItem usable || !usable.BattleOnly);
        m_inventoryEntry.OnAmountChanged += DisplayCount;
        m_inventoryEntry.OnMarkAsSeen += DisplayAsNew;
    }

    public override void SetAsSelected(bool selected)
    {
        base.SetAsSelected(selected);
        m_anim.SetBool("IsSelected", selected);
        m_selectionFrame.SetActive(false);
        m_selectionSplotch.SetActive(selected);
        m_shadow.SetActive(!selected);
    }

    public override void PauseSelection()
    {
        m_selectionFrame.SetActive(true);
        m_selectionSplotch.SetActive(false);
    }

    public override void ResetForPool()
    {
        if (m_inventoryEntry != null)
        {
            m_inventoryEntry.OnAmountChanged -= DisplayCount;
            m_inventoryEntry.OnMarkAsSeen -= DisplayAsNew;
        }

        SetAsSelected(false);
        m_inventoryEntry = null;
    }

    public override void SetAsSelectable(bool usable)
    {
        m_alphaGroup.alpha = usable ? USABLE_ALPHA : UNUSABLE_ALPHA;
    }

    private void DisplayItemData(Item item)
    {
        m_nameText.text = item.Name;
        m_descriptionText.text = ReplaceDescriptionTagWithColorTag(item.Description);
        m_icon.sprite = item.Icon;
        m_icon.gameObject.SetActive(m_icon.sprite != null);
    }

    private void DisplayStatsData(Equipment weapon)
    {
        m_mainTexts.ForEach(text => text.text = Helper.StringFormatting.PrettifyStat(weapon.MainModifier.Type));
        m_mainTextValues.ForEach(text => text.text = Helper.StringFormatting.FormatIntForUI(weapon.MainModifier.Amount, 3, text.color.g > 0.5f));

        m_secondaryTexts.ForEach(text => text.text = Helper.StringFormatting.PrettifyStat(weapon.SecondaryModifier.Type));
        m_secondaryTextValues.ForEach(text => text.text = Helper.StringFormatting.FormatIntForUI(weapon.SecondaryModifier.Amount, 2, text.color.g > 0.5f));
    }

    private void DisplayEffectsData(Equipment weapon)
    {
        m_effectContent.SetActive(weapon.Effect != null);

        if(weapon.Effect != null)
        {
            m_effectDescription.text = weapon.Effect.Description;
        }
    }

    private void DisplayCount(int amount)
    {
        m_countText.text = Helper.StringFormatting.FormatIntForUI(amount, 2, true);
    }

    private void DisplayAsNew(InventoryEntry entry)
    {
        m_newFlag.SetActive(entry.IsNew);
    }

    private string ReplaceDescriptionTagWithColorTag(string raw)
    {
        if (!raw.Contains("<m>"))
            return raw;

        string colorTag = ColorUtility.ToHtmlStringRGB(m_markupDescriptionColor);
        return raw.Replace("<m>", $"<color=#{colorTag}>");
    }
}
