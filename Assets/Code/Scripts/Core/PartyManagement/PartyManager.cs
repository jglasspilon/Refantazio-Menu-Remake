using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private PartyData m_partyData = new PartyData();

    [SerializeField]
    private CharacterSheet[] m_starterCharacters;

    [SerializeField]
    private CharacterSheet m_guideCharacter;

    private void Awake()
    {
        ObjectResolver.Instance.Register(m_partyData);      
    }

    private void Start()
    {
        foreach (CharacterSheet character in m_starterCharacters)
        {
            Character newCharacter = new Character(character);
            m_partyData.AddPartyMember(newCharacter);
        }

        m_partyData.InitializeGuide(m_guideCharacter);

        if (m_partyData.TryGetPartyMember(0, out Character leader))
        {
            leader.SetCharacterAsLeader();
        }
    }
}
