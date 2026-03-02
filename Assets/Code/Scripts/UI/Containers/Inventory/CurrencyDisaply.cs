using System;
using TMPro;
using UnityEngine;

public class CurrencyDisaply : MonoBehaviour
{
    [SerializeField]
    private ECurrencyType m_currenyType;

    [SerializeField]
    private TextMeshProUGUI m_text;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private InventoryData m_inventory;

    private void OnEnable()
    {
        m_inventory ??= ObjectResolver.Instance.Resolve((InventoryData inventory) => m_inventory = inventory);

        if (m_inventory == null)
        {
            Logger.LogError($"Failed to find Inventory data for Currency Display {m_currenyType}.", m_logProfile);
            return;
        }

        if (m_currenyType == ECurrencyType.Money)
        {
            m_inventory.OnMoneyChanged += HandleOnCurrencyChanged;
            HandleOnCurrencyChanged(m_inventory.Money, 0);
        }
        else 
        {
            m_inventory.OnMaglaChanged += HandleOnCurrencyChanged;
            HandleOnCurrencyChanged(m_inventory.Magla, 0);
        }
    }

    private void OnDisable()
    {
        if (m_inventory == null)
        {
            return;
        }

        if(m_currenyType == ECurrencyType.Money)
        {
            m_inventory.OnMoneyChanged -= HandleOnCurrencyChanged;
        }
        else
        {
            m_inventory.OnMaglaChanged -= HandleOnCurrencyChanged;
        }
    }

    private void HandleOnCurrencyChanged(int value, int delta)
    {
        m_text.text = value.ToString();
    }
}

