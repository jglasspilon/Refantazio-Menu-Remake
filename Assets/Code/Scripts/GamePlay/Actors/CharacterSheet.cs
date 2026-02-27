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

    protected override void OnValidate()
    {
        base.OnValidate();

        if(m_expCurve != null && Stats.Level == null)
        {
            Stats.InitializeLevel(new Level(m_expCurve));
        }

        Stats.Level.InitializeExp();
    }
}

[Serializable]
public class CharacterStats
{
    public event Action<Stat> OnStatChange;
    public event Action<int> OnLevelChange;

    public Level Level;
    public Stat HP = new Stat(StatType.HP, 0);
    public Stat MP = new Stat(StatType.MP, 0);
    public Stat Strength = new Stat(StatType.Strength, 0);
    public Stat Magic = new Stat(StatType.Magic, 0);
    public Stat Endurance = new Stat(StatType.Endurance, 0);
    public Stat Agility = new Stat(StatType.Agility, 0);
    public Stat Luck = new Stat(StatType.Luck, 0);
    public Stat Attack = new Stat(StatType.Attack, 0);
    public Stat Hit = new Stat(StatType.Hit, 0);
    public Stat Defence = new Stat(StatType.Defence, 0);
    public Stat Evasion = new Stat(StatType.Evasion, 0);

    private readonly Dictionary<StatType, Stat> m_typedStats;

    public CharacterStats()
    {
        HP.OnValueChange += HandleOnStatChange;
        MP.OnValueChange += HandleOnStatChange;
        Strength.OnValueChange += HandleOnStatChange;
        Magic.OnValueChange += HandleOnStatChange;
        Endurance.OnValueChange += HandleOnStatChange;
        Agility.OnValueChange += HandleOnStatChange;
        Luck.OnValueChange += HandleOnStatChange;

        m_typedStats = new Dictionary<StatType, Stat>()
        {
            {StatType.HP, HP},
            {StatType.MP, MP},
            {StatType.Strength, Strength},
            {StatType.Magic, Magic},
            {StatType.Endurance, Endurance},
            {StatType.Agility, Agility},
            {StatType.Luck, Luck},
            {StatType.Attack, Attack},
            {StatType.Hit, Hit},
            {StatType.Defence, Defence},
            {StatType.Evasion, Evasion}
        };
    }

    public void InitializeLevel(Level level)
    {
        Level = level;
        Level.OnLevelChange += HandleOnLevelChange;
    }

    private void HandleOnStatChange(Stat statChange)
    {
        OnStatChange?.Invoke(statChange);
    }

    private void HandleOnLevelChange(int levelChange)
    {
        OnLevelChange?.Invoke(levelChange);
    }
}

[Serializable]
public class Level
{
    public event Action<int> OnLevelChange;

    [SerializeField]
    private int m_value = 1;

    [SerializeField]
    private Resource m_exp;

    public int Value => m_value;
    public Resource Exp => m_exp;
    private AnimationCurveAsset m_expCurve;

    public Level(AnimationCurveAsset expCurve)
    {
        m_expCurve = expCurve;
        InitializeExp();
    }

    public void InitializeExp()
    {
        SetLevel(m_value);
    }

    public void AddExp(int amount, int level = 0)
    {
        int overflow = (m_exp.Current + amount) - m_exp.Max;

        if (overflow < 0 || m_value == 99)
        {
            m_exp.Apply(amount);

            if (level > 0)
            {
                OnLevelChange?.Invoke(level);
            }
            return;
        }

        LevelUp();
        AddExp(overflow, level++);
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
