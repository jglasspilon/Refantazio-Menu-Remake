using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem_MenuBasic : InventoryItem
{
    [SerializeField]
    private Image m_icon;

    [SerializeField]
    private TextMeshProUGUI m_nameText, m_countText, m_descriptionText;

    [SerializeField]
    private CanvasGroup m_alphaGroup;

    [SerializeField]
    private GameObject m_newFlag;

    [SerializeField]
    private Animator m_anim;

    [SerializeField]
    private Color m_healDescriptionColor, m_damageDescriptionColor;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private InventoryEntry m_inventoryEntry;
    private const float UNUSABLE_ALPHA = 0.5f;
    private const float USABLE_ALPHA = 1.0f;

    public override InventoryEntry InventoryEntry => m_inventoryEntry;

    public override void Initialize(InventoryEntry entry)
    {
        if(entry == null || entry.Item == null)
        {
            Logger.LogError("Generated an empty item. Initializing an empty item is not allowed.", m_logProfile);
            return;
        }

        transform.localScale = Vector3.one;
        m_inventoryEntry = entry;
        DisplayItemData(entry.Item);
        DisplayCount(entry.Count);
        DisplayAsNew();
        DisplayAsUsable(entry.Item is UsableItem usable && !usable.BattleOnly);
        m_inventoryEntry.OnAmountChanged += DisplayCount;
        m_inventoryEntry.OnMarkAsSeen += DisplayAsNew;
    }

    public override void SetAsSelected(bool selected)
    {
        base.SetAsSelected(selected);
        m_anim.SetBool("IsSelected", selected);
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

    private void DisplayAsUsable(bool usable)
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

    private void DisplayCount(int amount)
    {
        m_countText.text = Helper.StringFormatting.FormatIntForUI(amount, 2, true);
    }

    private void DisplayAsNew()
    {
        m_newFlag.SetActive(m_inventoryEntry.IsNew);
    }

    private string ReplaceDescriptionTagWithColorTag(string raw)
    {
        if (!raw.Contains("<m>") || m_inventoryEntry.Item is not UsableItem usable)
            return raw;

        string colorTag = usable.TargetingType.ToString().ToLower().Contains("foe") ? ColorUtility.ToHtmlStringRGB(m_damageDescriptionColor) : ColorUtility.ToHtmlStringRGB(m_healDescriptionColor);
        return raw.Replace("<m>", $"<color=#{colorTag}>");
    }
}
