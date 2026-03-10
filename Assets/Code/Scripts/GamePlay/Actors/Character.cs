using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Character: IPropertyProvider
{
    public event Action<Character> OnTypeChange;
    public event Action<EBattlePosition> OnBattlePositionChange;
    public event Action<Archetype> OnArchetypeChange;
    public event Action<bool> OnDeath;
    public event Action<int, int> OnLevelChange;
    public event Action OnCharacterUpdated;

    [SerializeField]
    private ECharacterType m_characterType;

    [SerializeField]
    private CharacterStats m_stats; 

    [SerializeField]
    private Resource m_health = new Resource(0);

    [SerializeField]
    private Resource m_mana = new Resource(0);

    [SerializeField]
    private Level m_level;

    [SerializeField]
    private EquipmentExecutor m_equipment;

    [SerializeField]
    private EBattlePosition m_battlePosition;

    [SerializeField]
    private List<Archetype> m_availableArchetypes = new List<Archetype>();

    private CharacterSheet m_characterBase;
    private Dictionary<string, IObservableProperty> m_properties;

    public string ID => m_characterBase.ID;
    public bool IsValid => m_characterBase != null;
    public string Name => m_characterBase.Name;
    public Mesh Mesh => m_characterBase.Mesh;
    public Sprite Banner => m_characterBase.BannerIcon;
    public Sprite Profile => m_characterBase.ProfileIcon;
    public Sprite Arm => m_characterBase.ArmIcon;
    public Level Level => m_level;
    public Resource HP => m_health;
    public Resource MP => m_mana;
    public Resource Exp => m_level.Exp;
    public CharacterStats Stats => m_stats;
    public EquipmentExecutor Equipment => m_equipment;
    public bool IsDead => m_health.Current == 0 && m_health.Max > 0;
    public EBattlePosition BattlePosition => m_battlePosition;
    public Skill[] Skills => m_equipment?.Archetype?.GetAvailableSkills() ?? Array.Empty<Skill>();
    public string CharacterType => m_characterType.ToString();
    public bool IsLeader => m_characterType == ECharacterType.Leader;
    public bool IsGuide => m_characterType == ECharacterType.Guide;
    public bool IsInActiveParty => m_characterType == ECharacterType.Party || IsLeader;

    #region Life Cycle Functions
    public Character(CharacterSheet sheet)
    {
        Archetype startArchetype = null;
        m_characterBase = sheet;
        m_stats = new CharacterStats(sheet.HP, sheet.MP, sheet.Str, sheet.Mag, sheet.End, sheet.Agi, sheet.Luck);
        m_level = sheet.CreateLevel();
        m_characterType = sheet.CharacterType;
        m_battlePosition = m_characterType == ECharacterType.Guide ? EBattlePosition.Undetermined : EBattlePosition.Front;

        if (m_characterBase.StartingArchetype != null)
        {
            startArchetype = new Archetype(m_characterBase.StartingArchetype);

            if(!m_availableArchetypes.Contains(startArchetype)) 
                m_availableArchetypes.Add(startArchetype);
        }

        m_equipment = new EquipmentExecutor(sheet.StartingWeapon, sheet.StartingArmor, sheet.StartingGear, sheet.StartingAccessory, startArchetype, this);
        ApplyHp(Stats.Endurance);
        ApplyMp(Stats.Magic);

        m_health.OnEmpty += HandleOnHealthEmpty;
        Stats.OnStatChange += HandleStatUpdate;
        Level.OnLevelChange += HandleLevelChange;
        Stats.Endurance.OnValueChange += ApplyHp;
        Stats.Magic.OnValueChange += ApplyMp;

        InitializeProperties();
    }
    #endregion

    #region Property Provision Functions
    private void InitializeProperties()
    {
        m_properties = Helper.DataHandling.BuildPropertyMap(this);
    }

    public bool TryGetProperty<T>(string key, out ObservableProperty<T> value)
    {
        Type type = typeof(T);
        if (m_properties.TryGetValue(key, out IObservableProperty raw) && raw is ObservableProperty<T> typed)
        {
            value = typed;
            return true;
        }

        value = null;
        return false;
    }
    #endregion

    #region CharacterType & Position Functions
    public void SetCharacterAsLeader()
    {
        m_characterType = ECharacterType.Leader;
        OnTypeChange?.Invoke(this);
    }

    public void SetCharacterToActiveParty()
    {
        m_characterType = ECharacterType.Party;
        OnTypeChange?.Invoke(this);
    }

    public void RemoveCharacterFromActiveParty()
    {
        m_characterType = m_characterBase.CharacterType;
        OnTypeChange?.Invoke(this);
    }

    public void SetCharacterBattlePosition(EBattlePosition battlePosition)
    {
        if (battlePosition == m_battlePosition || m_characterType == ECharacterType.Guide)
            return;

        m_battlePosition = battlePosition;
        OnBattlePositionChange?.Invoke(battlePosition);
    }
    #endregion

    #region Resource Functions
    public bool HasEnoughHealth(int amount)
    {
        return m_health.Current >= amount;
    }
    
    public void ApplyToHealth(int amount)
    {
        m_health.Apply(amount);
    }

    public bool HasEnoughMana(int amount)
    {
        return m_mana.Current >= amount;
    }

    public void ApplyToMana(int amount)
    {
        m_mana.Apply(amount);
    }

    private void HandleLevelChange(int level, int levelDelta)
    {
        OnLevelChange?.Invoke(level, levelDelta);
    }

    private void HandleOnHealthEmpty(bool isEmpty)
    {
        if(m_health.Max > 0)
            OnDeath?.Invoke(isEmpty);
    }
    #endregion

    #region Stats Functions
    private void ApplyHp(Stat endurance)
    {
        int value = Mathf.FloorToInt(Stats.HP.Value * m_level.Value * (1f + (endurance.Value / 100f)));
        m_health.SetMax(value, EResourceSetProcedure.Fill);
    }

    private void ApplyMp(Stat magic)
    {
        int value = Mathf.FloorToInt(Stats.MP.Value * m_level.Value * (1f + (magic.Value / 100f)));
        m_mana.SetMax(value, EResourceSetProcedure.Fill);
    }

    private void HandleStatUpdate(Stat statChanged)
    {
        OnCharacterUpdated?.Invoke();
    }
    #endregion
}
