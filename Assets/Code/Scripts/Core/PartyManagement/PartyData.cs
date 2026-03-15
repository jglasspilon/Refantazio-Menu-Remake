using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PartyData
{
    [SerializeField] private List<Character> m_orderedParty = new List<Character>();
    [SerializeField] private Character[] m_activeParty;

    public event Action OnPartyChanged, OnActivePartyChanged, OnLeaderRemoveAttempt, OnActivePartyOverloadAttempt;

    private Dictionary<string, Character> m_party = new Dictionary<string, Character>();
    private Character m_guide;
    private LoggingProfile m_logProfile;

    public bool ActivePartyFull { get { return !m_activeParty.Any(x => x == null || !x.IsValid); } }
    public Character Guide { get { return m_guide; } }

    public PartyData(LoggingProfile logProfile)
    {
        m_logProfile = logProfile;
        m_activeParty = new Character[] { null, null, null, null };
    }

    #region Party Member Management Functions
    public Character[] GetAllPartyMembers(bool includeGuide = true)
    {
        if(!includeGuide)
            return m_orderedParty.ToArray();

        List<Character> result = new List<Character>();
        result.AddRange(m_orderedParty);
        result.Add(m_guide);
        return result.ToArray();
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

    public bool TryGetActivePartyMember(string id, out Character character)
    {
        character = null;

        if (id == null)
        {
            Logger.LogError($"Failed to return active party member, null id was provided.", m_logProfile);
            return false;
        }

        for(int i = 0; i < m_activeParty.Length; i++)
        {
            Character charInSlot = m_activeParty[i];
            if (charInSlot == null || !charInSlot.IsValid)
                continue;

            if(charInSlot.ID == id)
            {
                character = charInSlot;
                return true;
            }
        }

        return false;
    }

    public bool TryGetActivePartyMember(int index, out Character character)
    {
        if (index >= m_activeParty.Length || index < 0)
        {
            Logger.LogError($"Failed to return party member from active party at index '{index}'. Index is out of bounds.", m_logProfile);
            character = null;
            return false;
        }

        character = m_activeParty[index];
        return character != null && character.IsValid;
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

        Logger.Log($"Added {newPartyMember.Name} to party.", m_logProfile);
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

        if(partyMember.IsLeader)
        {
            OnLeaderRemoveAttempt?.Invoke();
            Logger.Log("Cannot remove leader from party", m_logProfile);
        }

        m_party.Remove(partyMember.ID);
        m_orderedParty.Remove(partyMember);

        Logger.Log($"removed {partyMember.Name} from party.", m_logProfile);
        OnPartyChanged?.Invoke();

        if(TryGetActivePartyMember(partyMember.ID, out Character character))
        {
            RemoveActivePartyMember(partyMember);
        }
    }

    public void AddActivePartyMember(Character newActivePartyMember)
    {
        if (newActivePartyMember == null)
        {
            Logger.LogError($"Adding null active party members is not allowed.", m_logProfile);
            return;
        }

        if (TryGetActivePartyMember(newActivePartyMember.ID, out Character character))
        {
            Logger.LogError($"Failed adding new active party member '{newActivePartyMember.Name}'. Adding duplicate party members is not allowed.", m_logProfile);
            return;
        }

        if (ActivePartyFull)
        {
            OnActivePartyOverloadAttempt?.Invoke();
            Logger.Log($"Didn't add {newActivePartyMember.Name} to active party. Party limit of 4 already reached.", m_logProfile);
            return;
        }

        int slot = FindFirstEmptySlotInActiveParty();
        newActivePartyMember.SetCharacterToActiveParty();
        m_activeParty[slot] = newActivePartyMember;
        Logger.Log($"Added {newActivePartyMember.Name} to active party.", m_logProfile);
        OnActivePartyChanged?.Invoke();
    }

    private int FindFirstEmptySlotInActiveParty()
    {
        for (int i = 0; i < m_activeParty.Length; i++)
        {
            Character charInSlot = m_activeParty[i];
            if (charInSlot != null && charInSlot.IsValid)
                continue;

            return i;
        }

        return - 1;
    }

    public void RemoveActivePartyMember(Character activePartyMember)
    {
        if (activePartyMember == null)
        {
            Logger.LogError($"Removing null active party members is not allowed.", m_logProfile);
            return;
        }

        if (!TryGetActivePartyMember(activePartyMember.ID, out Character character))
        {
            Logger.LogError($"Failed removing party member '{activePartyMember.Name}'. Party member not found.", m_logProfile);
            return;
        }

        if(activePartyMember.IsLeader)
        {
            OnLeaderRemoveAttempt?.Invoke();
            Logger.Log("Cannot remove leader from active party", m_logProfile);
            return;
        }

        int slot = FindSlotForCharacterInActiveParty(activePartyMember);
        activePartyMember.RemoveCharacterFromActiveParty();
        m_activeParty[slot] = null;

        Logger.Log($"removed {activePartyMember.Name} from active party.", m_logProfile);
        OnActivePartyChanged?.Invoke();
    }

    private int FindSlotForCharacterInActiveParty(Character character)
    {
        for (int i = 0; i < m_activeParty.Length; i++)
        {
            Character charInSlot = m_activeParty[i];
            if (charInSlot == null || !charInSlot.IsValid)
                continue;

            if (charInSlot.ID == character.ID)
                return i;
        }

        return -1;
    }

    public void InitializeGuide(CharacterSheet guide)
    {
        m_guide = new Character(guide);
        Logger.Log($"Set {m_guide.Name} as party guide.", m_logProfile);
    }
    #endregion
}
