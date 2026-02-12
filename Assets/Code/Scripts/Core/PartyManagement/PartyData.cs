using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartyData
{
    public event Action OnPartyChanged, OnActivePartyChanged;

    [SerializeField]
    private List<Character> m_party = new List<Character>();

    [SerializeField]
    private List<Character> m_activeParty = new List<Character>();

    [SerializeField]
    private LoggingProfile m_logProfile;

    private const int ACTIVE_PARTY_LIMIT = 4;

    public bool ActivePartyFull { get { return m_activeParty.Count >= 4; } }

    public Character[] GetAllPartyMembers()
    {
        return m_party.ToArray();
    }

    public Character[] GetAllActivePartyMembers()
    {
        return m_activeParty.ToArray();
    }

    public Character GetPartyMember(int index)
    {
        if(index >= m_party.Count || index < 0)
        {
            Logger.LogError($"Failed to return party member at index '{index}'. Index is out of bounds.", m_logProfile);
            return null;
        }

        return m_party[index];
    }

    public Character GetActivePartyMember(int index)
    {
        if (index >= m_activeParty.Count || index < 0)
        {
            Logger.LogError($"Failed to return party member from active party at index '{index}'. Index is out of bounds.", m_logProfile);
            return null;
        }

        return m_activeParty[index];
    }

    public void AddPartyMember(Character newPartyMember)
    {
        if(newPartyMember == null)
        {
            Logger.LogError($"Adding null party members is not allowed.", m_logProfile);
            return;
        }

        if(m_party.Contains(newPartyMember))
        {
            Logger.LogError($"Failed adding new party member '{newPartyMember.Name}'. Adding duplicate party members is not allowed.", m_logProfile);
            return;
        }

        m_party.Add(newPartyMember);
        OnPartyChanged?.Invoke();

        if(!ActivePartyFull)
        {
            AddActivePartyMember(newPartyMember);
        }
    }

    public void RemovePartyMember(Character partyMember)
    {
        if (partyMember == null)
        {
            Logger.LogError($"Removing null party members is not allowed.", m_logProfile);
            return;
        }

        if (!m_party.Contains(partyMember))
        {
            Logger.LogError($"Failed removing party member '{partyMember.Name}'. Party member not found.", m_logProfile);
            return;
        }

        m_party.Remove(partyMember);
        OnPartyChanged?.Invoke();
    }

    public void AddActivePartyMember(Character newActivePartyMember)
    {
        if (newActivePartyMember == null)
        {
            Logger.LogError($"Adding null active party members is not allowed.", m_logProfile);
            return;
        }

        if (m_activeParty.Contains(newActivePartyMember))
        {
            Logger.LogError($"Failed adding new active party member '{newActivePartyMember.Name}'. Adding duplicate party members is not allowed.", m_logProfile);
            return;
        }

        if (m_activeParty.Count >= ACTIVE_PARTY_LIMIT)
        {
            Logger.Log($"Didn't add {newActivePartyMember.Name} to active party. Party limit of {ACTIVE_PARTY_LIMIT} already reached.", m_logProfile);
            return;
        }

        m_activeParty.Add(newActivePartyMember);
        OnActivePartyChanged?.Invoke();
    }

    public void RemoveActivePartyMember(Character activePartyMember)
    {
        if (activePartyMember == null)
        {
            Logger.LogError($"Removing null active party members is not allowed.", m_logProfile);
            return;
        }

        if (!m_activeParty.Contains(activePartyMember))
        {
            Logger.LogError($"Failed removing party member '{activePartyMember.Name}'. Party member not found.", m_logProfile);
            return;
        }

        m_activeParty.Remove(activePartyMember);
        OnActivePartyChanged?.Invoke();
    }
}
