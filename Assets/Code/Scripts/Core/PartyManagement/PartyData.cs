using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartyData
{
    public event Action OnPartyChanged, OnActivePartyChanged;

    [SerializeField]
    private LoggingProfile m_logProfile;

    [SerializeField]
    private List<Character> m_orderedParty = new List<Character>();

    [SerializeField]
    private List<Character> m_orderedActiveParty = new List<Character>();

    private Dictionary<string, Character> m_party = new Dictionary<string, Character>();
    private Dictionary<string, Character> m_activeParty = new Dictionary<string, Character>();   
    private Character m_guide;

    private const int ACTIVE_PARTY_LIMIT = 4;

    public bool ActivePartyFull { get { return m_activeParty.Count >= 4; } }
    public Character Guide { get { return m_guide; } }

    public Character[] GetAllPartyMembers()
    {
        return m_orderedParty.ToArray();
    }

    public Character[] GetAllActivePartyMembers()
    {
        return m_orderedActiveParty.ToArray();
    }

    public bool TryGetPartyMember(string id, out Character character)
    {
        if(id == null)
        {
            Logger.LogError($"Failed to return party member, null id was provided.", m_logProfile);
            character = null;
            return false;
        }

        if(!m_party.TryGetValue(id, out character))
        {
            Logger.LogError($"Failed to return party member, provided character id '{id}' was not recognized.", m_logProfile);
            character = null;
            return false;
        }

        return true;
    }

    public bool TryGetPartyMember(int index, out Character character)
    {
        if(index >= m_party.Count || index < 0)
        {           
            Logger.LogError($"Failed to return party member at index '{index}'. Index is out of bounds.", m_logProfile);
            character = null;
            return false;
        }

        character = GetAllPartyMembers()[index];
        return true;
    }

    public bool TryGetActivePartyMember(int index, out Character character)
    {
        if (index >= m_activeParty.Count || index < 0)
        {
            Logger.LogError($"Failed to return party member from active party at index '{index}'. Index is out of bounds.", m_logProfile);
            character = null;
            return false;
        }

        character = GetAllActivePartyMembers()[index];
        return true;
    }

    public void AddPartyMember(Character newPartyMember)
    {
        if(newPartyMember == null)
        {
            Logger.LogError($"Adding null party members is not allowed.", m_logProfile);
            return;
        }

        if(m_party.ContainsKey(newPartyMember.ID))
        {
            Logger.LogError($"Failed adding new party member '{newPartyMember.Name}'. Adding duplicate party members is not allowed.", m_logProfile);
            return;
        }

        m_party.Add(newPartyMember.ID, newPartyMember);
        m_orderedParty.Add(newPartyMember);
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

        if (!m_party.ContainsKey(partyMember.ID))
        {
            Logger.LogError($"Failed removing party member '{partyMember.Name}'. Party member not found.", m_logProfile);
            return;
        }

        m_party.Remove(partyMember.ID);
        m_orderedParty.Remove(partyMember);
        OnPartyChanged?.Invoke();
    }

    public void AddActivePartyMember(Character newActivePartyMember)
    {
        if (newActivePartyMember == null)
        {
            Logger.LogError($"Adding null active party members is not allowed.", m_logProfile);
            return;
        }

        if (m_activeParty.ContainsKey(newActivePartyMember.ID))
        {
            Logger.LogError($"Failed adding new active party member '{newActivePartyMember.Name}'. Adding duplicate party members is not allowed.", m_logProfile);
            return;
        }

        if (m_activeParty.Count >= ACTIVE_PARTY_LIMIT)
        {
            Logger.Log($"Didn't add {newActivePartyMember.Name} to active party. Party limit of {ACTIVE_PARTY_LIMIT} already reached.", m_logProfile);
            return;
        }

        newActivePartyMember.SetCharacterToActiveParty();
        m_activeParty.Add(newActivePartyMember.ID, newActivePartyMember);
        m_orderedActiveParty.Add(newActivePartyMember);
        OnActivePartyChanged?.Invoke();
    }

    public void RemoveActivePartyMember(Character activePartyMember)
    {
        if (activePartyMember == null)
        {
            Logger.LogError($"Removing null active party members is not allowed.", m_logProfile);
            return;
        }

        if (!m_activeParty.ContainsKey(activePartyMember.ID))
        {
            Logger.LogError($"Failed removing party member '{activePartyMember.Name}'. Party member not found.", m_logProfile);
            return;
        }

        activePartyMember.RemoveCharacterFromActiveParty();
        m_activeParty.Remove(activePartyMember.ID);
        m_orderedActiveParty.Remove(activePartyMember);
        OnActivePartyChanged?.Invoke();
    }

    public void InitializeGuide(CharacterSheet guide)
    {
        m_guide = new Character(guide);
    }
}
