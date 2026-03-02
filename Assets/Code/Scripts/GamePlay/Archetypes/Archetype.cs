using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Archetype
{
    public event Action<int, int> OnRankChange;

    [SerializeField]
    private ArchetypeData m_baseData;

    [SerializeField]
    private Level m_rank;

    [SerializeField]
    private Skill[] m_availableSkills;

    private Dictionary<int, Skill[]> m_skillsByRank;

    public string Name => m_baseData.name;
    public Sprite Icon => m_baseData.Icon;
    public Mesh Mesh => m_baseData.Mesh;
    public Level Rank => m_rank;

    public Archetype(ArchetypeData baseData)
    { 
        m_baseData = baseData;
        m_rank = new Level(1, baseData.RankExpCurve);
        m_skillsByRank = baseData.SkillsByRank.ToDictionary(x => x.Key, x => x.Value);
        m_availableSkills = GetAvailableSkills();
    }

    public Skill[] GetAvailableSkills()
    {
        return m_skillsByRank.Where(x => x.Key <= m_rank.Value).SelectMany(x => x.Value).ToArray();
    }
}
