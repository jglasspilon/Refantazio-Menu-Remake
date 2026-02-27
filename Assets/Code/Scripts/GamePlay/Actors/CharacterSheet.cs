using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Characters")]
public class CharacterSheet : UniqueScriptableObject
{
    [SerializeField]
    private ECharacterType m_characterType;

    [SerializeField]
    private string m_name;

    [SerializeField]
    private ArchetypeData m_startingArchetype;

    [SerializeField]
    private AnimationCurveAsset m_expCurve;

    [SerializeField]
    private int m_startingLevel;

    [SerializeField]
    private CharacterStats m_stats;

    [SerializeField]
    private Skill[] m_startingSkills;

    [SerializeField]
    private Sprite m_profileIcon, m_bannerIcon, m_armIcon;

    [SerializeField]
    private Mesh m_mesh;

    public ECharacterType CharacterType => m_characterType;
    public string Name => m_name;
    public ArchetypeData StartingArchetype => m_startingArchetype;
    public CharacterStats Stats => m_stats;
    public Skill[] StartingSkills => m_startingSkills;
    public Sprite ProfileIcon => m_profileIcon;
    public Sprite BannerIcon => m_bannerIcon;
    public Sprite ArmIcon => m_armIcon;
    public Mesh Mesh => m_mesh;

    public Level CreateLevel()
    {
        return new Level(m_startingLevel, m_expCurve);
    }
}

[Serializable]
public class CharacterStats
{
    public event Action<Stat> OnStatChange;

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

    public CharacterStats(CharacterStats refStats)
    {
        HP = new Stat(refStats.HP.Type, refStats.HP.BaseValue);
        MP = new Stat(refStats.MP.Type, refStats.MP.BaseValue);
        Strength = new Stat(refStats.Strength.Type, refStats.Strength.BaseValue);
        Magic = new Stat(refStats.Magic.Type, refStats.Magic.BaseValue);
        Endurance = new Stat(refStats.Endurance.Type, refStats.Endurance.BaseValue);
        Agility = new Stat(refStats.Agility.Type, refStats.Agility.BaseValue);
        Luck = new Stat(refStats.Luck.Type, refStats.Luck.BaseValue);
        Attack = new Stat(refStats.Attack.Type, refStats.Attack.BaseValue);
        Hit = new Stat(refStats.Hit.Type, refStats.Hit.BaseValue);
        Defence = new Stat(refStats.Defence.Type, refStats.Defence.BaseValue);
        Evasion = new Stat(refStats.Evasion.Type, refStats.Evasion.BaseValue);

        InitializeStats();
    }

    private void InitializeStats()
    {
        HP.OnValueChange += HandleOnStatChange;
        MP.OnValueChange += HandleOnStatChange;
        Strength.OnValueChange += HandleOnStatChange;
        Magic.OnValueChange += HandleOnStatChange;
        Endurance.OnValueChange += HandleOnStatChange;
        Agility.OnValueChange += HandleOnStatChange;
        Luck.OnValueChange += HandleOnStatChange;
    }

    private void HandleOnStatChange(Stat statChange)
    {
        OnStatChange?.Invoke(statChange);
    }
}

[Serializable]
public class Level
{
    public event Action<int, int> OnLevelChange;

    [SerializeField]
    private int m_value = 1;

    [SerializeField]
    private Resource m_exp;

    public int Value => m_value;
    public Resource Exp => m_exp;
    private AnimationCurveAsset m_expCurve;

    public Level(int value, AnimationCurveAsset expCurve)
    {
        m_value = value;
        m_expCurve = expCurve;
        InitializeExp();
    }

    public void InitializeExp()
    {
        SetLevel(m_value);
    }

    public void AddExp(int amount, int levelDelta = 0)
    {
        int overflow = (m_exp.Current + amount) - m_exp.Max;

        if (overflow < 0 || m_value == 99)
        {
            m_exp.Apply(amount);

            if (levelDelta > 0)
            {
                OnLevelChange?.Invoke(m_value, levelDelta);
            }
            return;
        }

        LevelUp();
        AddExp(overflow, levelDelta + 1);
    }

    public void LevelUp()
    {
        SetLevel(m_value + 1);
    }

    private void SetLevel(int level)
    {
        if (m_expCurve == null)
            return;

        level = Mathf.Clamp(level, 1, 99);
        m_exp = new Resource((int)m_expCurve.Evaluate(level));
        m_value = level;

        if (m_value == 99)
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
