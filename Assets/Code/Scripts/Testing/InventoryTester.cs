using System;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [SerializeField]
    private EMode m_testingMode = EMode.Money;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private InventoryData m_inventoryData;


    private const int TEST_AMOUNT = 237;
    

    private void Start()
    {
        if(!ObjectResolver.Instance.TryResolve((inventoryData) => m_inventoryData = inventoryData, out m_inventoryData))
        {
            Logger.Log("No ivnentory data on start", gameObject, m_logProfile);
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.KeypadPeriod))
        {
            m_testingMode = m_testingMode == EMode.Money ? EMode.Magla : EMode.Money;
            Logger.Log($"Changed to {m_testingMode} mode.", gameObject, m_logProfile);
        }

        if(Input.GetKeyUp(KeyCode.KeypadPlus))
        {
            if(m_testingMode == EMode.Money)
                m_inventoryData.ApplyMoney(TEST_AMOUNT);
            else
                m_inventoryData.ApplyMagla(TEST_AMOUNT);

            Logger.Log($"Added {TEST_AMOUNT} {m_testingMode}.", gameObject, m_logProfile);
        }

        if( Input.GetKeyUp(KeyCode.KeypadMinus))
        {
            if (m_testingMode == EMode.Money)
                m_inventoryData.ApplyMoney(-TEST_AMOUNT);
            else
                m_inventoryData.ApplyMagla(-TEST_AMOUNT);

            Logger.Log($"Removed {TEST_AMOUNT} {m_testingMode}.", gameObject, m_logProfile);
        }        
    }

    public enum EMode
    {
        Money,
        Magla,
    }
}


