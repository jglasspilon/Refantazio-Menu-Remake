using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Character")]
public class CharacterSheet : UniqueScriptableObject
{
    [SerializeField] private ECharacterType m_characterType;
    [SerializeField] private string m_name;
    [SerializeField] private ArchetypeData m_startingArchetype;
    [SerializeField] private ArchetypeData[] m_availableArchetypes;
    [SerializeField] private AnimationCurveAsset m_expCurve;

    [Header("Level and Stats:")]
    [SerializeField] private int m_startingLevel;
    [SerializeField] private int m_hp, m_mp, m_str, m_mag, m_end, m_agi, m_luck;

    [Header("Starting Equipment:")]
    [SerializeField] private EquipmentData m_startingWeapon;
    [SerializeField] private EquipmentData m_startingArmor, m_startingGear, m_startingAccesory;

    [Header("Display Content:")]
    [SerializeField] private Mesh m_mesh;
    [SerializeField] private Sprite m_profileIcon, m_bannerIcon, m_armIcon;  

    public ECharacterType CharacterType => m_characterType;
    public string Name => m_name;
    public ArchetypeData StartingArchetype => m_startingArchetype;
    public ArchetypeData[] AvailableArchetypes => m_availableArchetypes;
    public EquipmentData StartingArmor => m_startingArmor;
    public EquipmentData StartingWeapon => m_startingWeapon;
    public EquipmentData StartingGear => m_startingGear;
    public EquipmentData StartingAccessory => m_startingAccesory;
    public int HP => m_hp;
    public int MP => m_mp;
    public int Str => m_str;
    public int Mag => m_mag;
    public int End => m_end;
    public int Agi => m_agi;
    public int Luck => m_luck;
    public Sprite ProfileIcon => m_profileIcon;
    public Sprite BannerIcon => m_bannerIcon;
    public Sprite ArmIcon => m_armIcon;
    public Mesh Mesh => m_mesh;

    public Level CreateLevel()
    {
        return new Level(m_startingLevel, 99, m_expCurve);
    }
}

[Serializable]
public class CharacterStats: ISubPropertyProvider
{
    public Stat HP;
    public Stat MP;
    public Stat Strength;
    public Stat Magic;
    public Stat Endurance;
    public Stat Agility;
    public Stat Luck;
    public Stat Attack;
    public Stat Hit;
    public Stat Defence;
    public Stat Evasion;

    public CharacterStats(int hp, int mp, int str, int mag, int end, int agi, int luck)
    {
        HP = new Stat(EStatType.HP, hp);
        MP = new Stat(EStatType.MP, mp);
        Strength = new Stat(EStatType.Strength, str);
        Magic = new Stat(EStatType.Magic, mag);
        Endurance = new Stat(EStatType.Endurance, end);
        Agility = new Stat(EStatType.Agility, agi);
        Luck = new Stat(EStatType.Luck, luck);
        Attack = new Stat(EStatType.Attack, 0);
        Hit = new Stat(EStatType.Hit, 0);
        Defence = new Stat(EStatType.Defence, 0);
        Evasion = new Stat(EStatType.Evasion, 0);
    }

    public Stat GetStat(EStatType statType)
    {
        switch (statType)
        {
            case EStatType.Attack: return Attack;
            case EStatType.Hit: return Hit;
            case EStatType.Defence: return Defence;
            case EStatType.Evasion: return Evasion;
            case EStatType.HP: return HP;
            case EStatType.MP: return MP;
            case EStatType.Strength: return Strength;
            case EStatType.Magic: return Magic;
            case EStatType.Endurance: return Endurance;
            case EStatType.Agility: return Agility;
            case EStatType.Luck: return Luck;
        }

        Debug.LogError($"Failed to return stat {statType}");
        return null;
    }

    public IEnumerable<KeyValuePair<string, IObservableProperty>> GetSubProperties(string parentKey)
    {
        return Helper.DataHandling.GetObservableFields(this, parentKey);
    }
}

[Serializable]
public class Level: ObservableProperty<int>, ISubPropertyProvider
{
    [SerializeField] private Resource m_exp = new Resource(0);

    public event Action<int, int> OnLevelChange;

    public Resource Exp => m_exp;
    private AnimationCurveAsset m_expCurve;
    private int m_max;

    public Level(int value, int max, AnimationCurveAsset expCurve)
    {
        Value = value;
        m_max = max;
        m_expCurve = expCurve;
        InitializeExp();
    }

    public IEnumerable<KeyValuePair<string, IObservableProperty>> GetSubProperties(string parentKey)
    {
        return Helper.DataHandling.GetObservableFields(this, parentKey);
    }

    public void InitializeExp()
    {
        SetLevel(Value);
    }

    public void AddExp(int amount, int levelDelta = 0)
    {
        int overflow = (m_exp.Current + amount) - m_exp.Max;

        if (overflow < 0 || Value == m_max)
        {
            m_exp.Apply(amount);

            if (levelDelta > 0)
            {
                OnLevelChange?.Invoke(Value, levelDelta);
            }
            return;
        }

        LevelUp();
        AddExp(overflow, levelDelta + 1);
    }

    public void LevelUp()
    {
        SetLevel(Value + 1);
    }

    private void SetLevel(int level)
    {
        if (m_expCurve == null)
            return;

        level = Mathf.Clamp(level, 1, m_max);
        m_exp.SetMax((int)m_expCurve.Evaluate(level), EResourceSetProcedure.Reset);
        m_exp.Apply(-m_exp.Current);
        Value = level;

        if (Value == m_max)
        {
            m_exp.Apply(m_exp.Max);
            return;
        }
    }
}

public enum ECharacterType
{
    Leader,
    Reserve,
    Party,
    Guide,
    Enemy
}
