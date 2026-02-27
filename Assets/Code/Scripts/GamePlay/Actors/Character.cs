using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character
{
    public event Action<ECharacterType> OnTypeChange;
    public event Action<EBattlePosition> OnBattlePositionChange;
    public event Action<Archetype> OnArchetypeChange;
    public event Action<bool> OnDeath;
    public event Action<int, int> OnLevelChange;
    public event Action OnCharacterUpdated;

    [SerializeField]
    private string m_name;

    [SerializeField]
    private ECharacterType m_characterType;

    [SerializeField]
    private CharacterStats m_stats; 

    [SerializeField]
    private Resource m_health = new Resource();

    [SerializeField]
    private Resource m_mana = new Resource();

    [SerializeField]
    private Level m_level;

    [SerializeField]
    private Archetype m_equipedArchetype;

    [SerializeField]
    private List<Archetype> m_availableArchetypes = new List<Archetype>();

    [SerializeField]
    private EBattlePosition m_battlePosition;

    private CharacterSheet m_characterBase;

    public string ID => m_characterBase.ID;
    public bool IsValid => m_characterBase != null;
    public ECharacterType CharacterType => m_characterType;
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
    public Archetype EquipedArchetype => m_equipedArchetype;
    public bool IsDead => m_health.Current == 0 && m_health.Max > 0;
    public EBattlePosition BattlePosition => m_battlePosition;

    #region Life Cycle Functions
    public Character(CharacterSheet sheet)
    {
        m_characterBase = sheet;
        m_name = sheet.Name;
        m_stats = new CharacterStats(sheet.Stats);
        m_level = sheet.CreateLevel();
        m_characterType = sheet.CharacterType;
        m_battlePosition = m_characterType == ECharacterType.Guide ? EBattlePosition.Undetermined : EBattlePosition.Front;
        m_equipedArchetype = new Archetype(m_characterBase.StartingArchetype);
        ApplyStats();

        HP.OnEmpty += HandleOnHealthEmpty;
        HP.OnResourceChange += HandleOnHealthChanged;
        MP.OnResourceChange += HandleOnManaChanged;
        Stats.OnStatChange += HandleStatUpdate;
        Level.OnLevelChange += HandleLevelChange;
    }

    public void LoadCharacterData(Character data)
    {
        if(data == null)
        {
            Archetype defaultArchetype = new Archetype(m_characterBase.StartingArchetype);
            m_availableArchetypes.Add(defaultArchetype);
            m_equipedArchetype = defaultArchetype;
        }

        //TODO: load saved character data 
    }
    #endregion

    #region CharacterType & Position Functions
    public void SetCharacterAsLeader()
    {
        m_characterType = ECharacterType.Leader;
        OnTypeChange?.Invoke(m_characterType);
    }

    public void SetCharacterToActiveParty()
    {
        m_characterType = ECharacterType.Party;
        OnTypeChange?.Invoke(m_characterType);
    }

    public void RemoveCharacterFromActiveParty()
    {
        m_characterType = m_characterBase.CharacterType;
        OnTypeChange?.Invoke(m_characterType);
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
    
    public void ApplyHealth(int amount)
    {
        m_health.Apply(amount);
    }

    public bool HasEnoughMana(int amount)
    {
        return m_mana.Current >= amount;
    }

    public void ApplyMana(int amount)
    {
        m_mana.Apply(amount);
    }

    public void ApplyExp(int amount)
    {
        m_level.AddExp(amount);
    }

    private void HandleOnHealthChanged(int amount, float proportion, int delta)
    {
        OnCharacterUpdated?.Invoke();
    }

    private void HandleOnManaChanged(int amount, float proportion, int delta)
    {
        OnCharacterUpdated?.Invoke();
    }

    private void HandleOnHealthEmpty(bool isEmpty)
    {
        OnDeath?.Invoke(isEmpty);
    }
    #endregion

    #region Stats Functions
    private void ApplyStats()
    {
        m_health.SetMax(Stats.HP.Value, true);
        m_mana.SetMax(Stats.MP.Value, true);
    }

    private void HandleStatUpdate(Stat statChanged)
    {
        OnCharacterUpdated?.Invoke();
    }

    private void HandleLevelChange(int level, int levelDelta)
    {
        OnLevelChange?.Invoke(level, levelDelta);
    }
    #endregion
}
