using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private PartyData m_partyData = new PartyData();

    [SerializeField]
    private CharacterSheet[] m_starterCharacter;

    private void Awake()
    {
        ObjectResolver.Instance.Register(m_partyData);

        foreach(CharacterSheet character in m_starterCharacter)
        {
            Character newCharacter = new Character(character);
            m_partyData.AddPartyMember(newCharacter);
        }
    }
}
