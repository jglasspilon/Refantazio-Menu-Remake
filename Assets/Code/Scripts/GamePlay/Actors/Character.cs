using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character
{
    [SerializeField]
    private CharacterSheet m_characterBase;

    [SerializeField]
    private Health m_health;

    [SerializeField]
    private Mana m_mana;

    [SerializeField]
    private CharacterStats m_leveledStats = new CharacterStats();

    [SerializeField]
    private Archetype m_equipedArchetype;

    [SerializeField]
    private List<Archetype> m_availableArchetypes = new List<Archetype>();

    public string Name => m_characterBase.Name;

    public Character(CharacterSheet sheet)
    {
        m_characterBase = sheet;
    }

    public void LoadCharacterData(Character data)
    {
        if(data == null)
        {
            Archetype defaultArchetype = new Archetype(m_characterBase.StartingArchetype);
            m_availableArchetypes.Add(defaultArchetype);
            m_equipedArchetype = defaultArchetype;
        }

        //TODO: load all saved character data 
    }
}
