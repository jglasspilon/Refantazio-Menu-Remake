using System;
using UnityEngine;

[Serializable]
public class Archetype
{
    [SerializeField]
    private ArchetypeData m_baseData;

    [SerializeField]
    private int m_rank = 1;

    public Archetype(ArchetypeData baseData)
    { 
        m_baseData = baseData; 
    }
}
