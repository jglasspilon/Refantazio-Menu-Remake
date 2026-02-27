using System;
using UnityEngine;

[Serializable]
public class Archetype
{
    [SerializeField]
    private ArchetypeData m_baseData;

    [SerializeField]
    private int m_rank;

    public string Name => m_baseData.name;
    public Sprite Icon => m_baseData.Icon;
    public Mesh Mesh => m_baseData.Mesh;

    public Archetype(ArchetypeData baseData)
    { 
        m_baseData = baseData;
        m_rank = 1;
    }
}
