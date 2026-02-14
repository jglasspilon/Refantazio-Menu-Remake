using System;
using UnityEngine;

public class PartyTester : MonoBehaviour
{
    [SerializeField]
    private EMode m_testingMode = EMode.Damage;

    [SerializeField]
    private CharacterSheet m_characterToAdd;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private PartyData m_partyData;

    private const int TEST_AMOUNT = 30;
    

    private void Start()
    {
        if(!ObjectResolver.Instance.TryResolve((partyData) => m_partyData = partyData, out m_partyData))
        {
            Logger.Log("No party data on start", gameObject, m_logProfile);
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.H))
        {
            m_testingMode = EMode.Heal;
            Logger.Log("Changed to Healing mode.", gameObject, m_logProfile);
        }

        if(Input.GetKeyUp(KeyCode.D))
        {
            m_testingMode = EMode.Damage;
            Logger.Log("Changed to Damage mode.", gameObject, m_logProfile);
        }

        if( Input.GetKeyUp(KeyCode.R))
        {
            m_testingMode = EMode.RemoveFromParty;
            Logger.Log("Changed to Remove mode.", gameObject, m_logProfile);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            m_testingMode = EMode.AddToActiveParty;
            Logger.Log($"Changed to Add to Active Party mode.", gameObject, m_logProfile);
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            m_testingMode = EMode.RemoveFromActiveParty;
            Logger.Log($"Changed to Remove from Active Party mode.", gameObject, m_logProfile);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            m_partyData.AddPartyMember(new Character(m_characterToAdd));
            Logger.Log($"Adding {m_characterToAdd.Name} to party.", gameObject, m_logProfile);            
        }

        

        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            TestAllPartyMembers();
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            TestPartyMember(0);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            TestPartyMember(1);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            TestPartyMember(2);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            TestPartyMember(3);
        }

        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            TestPartyMember(4);
        }

        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            TestPartyMember(5);
        }

        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            TestPartyMember(6);
        }

        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            TestPartyMember(7);
        }
    }

    private void TestPartyMember(int index)
    {
        switch (m_testingMode)
        {
            case EMode.Heal:
                if(m_partyData.TryGetPartyMember(index, out Character characterToHeal))
                {
                    characterToHeal.ApplyHealth(TEST_AMOUNT);
                }
                break;
            case EMode.Damage:
                if (m_partyData.TryGetPartyMember(index, out Character characterToDamage))
                {
                    characterToDamage.ApplyHealth(-TEST_AMOUNT);
                }
                break;
            case EMode.RemoveFromParty:
                if (m_partyData.TryGetPartyMember(index, out Character characterToRemove))
                {
                    m_partyData.RemovePartyMember(characterToRemove);
                }
                break;
            case EMode.RemoveFromActiveParty:
                if (m_partyData.TryGetPartyMember(index, out Character characterToRemoveActive))
                {
                    m_partyData.RemoveActivePartyMember(characterToRemoveActive);
                }
                break;
            case EMode.AddToActiveParty:
                if (m_partyData.TryGetPartyMember(index, out Character characterToAddActive))
                {
                    m_partyData.AddActivePartyMember(characterToAddActive);
                }
                break;
        }
    }

    private void TestAllPartyMembers()
    {
        switch (m_testingMode)
        {
            case EMode.Heal:
                foreach(Character character in m_partyData.GetAllPartyMembers())
                {
                    character.ApplyHealth(TEST_AMOUNT);
                }
                break;
            case EMode.Damage:
                foreach (Character character in m_partyData.GetAllPartyMembers())
                {
                    character.ApplyHealth(-TEST_AMOUNT);
                }
                break;
        }
    }

    public enum EMode
    {
        Heal,
        Damage,
        RemoveFromParty,
        RemoveFromActiveParty,
        AddToActiveParty
    }
}


