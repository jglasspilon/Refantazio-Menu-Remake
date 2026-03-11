using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Archetype: ISubPropertyProvider
{
    public event Action<StatModifier[], StatModifier[]> OnStatModifiersChanged;

    [SerializeField] private ArchetypeData m_baseData;
    [SerializeField] private Level m_rank;
    [SerializeField] private EEquipmentType m_weaponType;
    [SerializeField] private Equipment m_defaultWeapon;
    [SerializeField] private Skill[] m_availableSkills;
    [SerializeField] private ObservableProperty<Sprite> m_icon = new ObservableProperty<Sprite>();

    private Dictionary<EStatType, StatModifier> m_statModifiers;
    private Dictionary<int, Skill[]> m_skillsByRank;

    public string Name => m_baseData.name;
    public Mesh Mesh => m_baseData.Mesh;
    public Level Rank => m_rank;
    public EEquipmentType EquipableWeaponType => m_weaponType;
    public Equipment DefaultWeapon => m_defaultWeapon;
    public StatModifier[] StatModifiers => m_statModifiers.Values.ToArray();

    public Archetype(ArchetypeData baseData)
    { 
        m_baseData = baseData;
        m_rank = new Level(1, 20, baseData.RankExpCurve);
        m_statModifiers = baseData.RankedStatCurves.ToDictionary(x => x.Key, x => new StatModifier(x.Key, (int)x.Value.Evaluate(m_rank.Value)));
        m_skillsByRank = baseData.SkillsByRank.ToDictionary(x => x.Key, x => x.Value);
        m_weaponType = baseData.EquipableWeaponType;
        m_defaultWeapon = baseData.DefaultWeapon;
        m_icon.Value = baseData.Icon;
        m_availableSkills = GetAvailableSkills();
        m_rank.OnLevelChange += HandleRankUp;
    }

    public IEnumerable<KeyValuePair<string, IObservableProperty>> GetSubProperties(string parentKey)
    {
        return Helper.DataHandling.GetObservableFields(this, parentKey);
    }

    public Skill[] GetAvailableSkills()
    {
        return m_skillsByRank.Where(x => x.Key <= m_rank.Value).SelectMany(x => x.Value).ToArray();
    }

    public StatModifier GetStatModifier(EStatType statType)
    {
        if (m_statModifiers.TryGetValue(statType, out StatModifier modifier))
            return modifier;

        else
            return null;
    }

    private void HandleRankUp(int value, int delta)
    {
        Dictionary<EStatType, StatModifier> oldModifiers = m_statModifiers;
        m_statModifiers = m_baseData.RankedStatCurves.ToDictionary(x => x.Key, x => new StatModifier(x.Key, (int)x.Value.Evaluate(m_rank.Value)));
        OnStatModifiersChanged(oldModifiers.Values.ToArray(), m_statModifiers.Values.ToArray());
    }
}
