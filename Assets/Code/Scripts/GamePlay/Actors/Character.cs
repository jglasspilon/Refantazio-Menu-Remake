using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character: IDisposable
{
    public event Action<int, float> OnHealthChanged, OnManaChanged;
    public event Action<ECharacterType> OnTypeChange;

    [SerializeField]
    private CharacterSheet m_characterBase;

    [SerializeField]
    private ECharacterType m_characterType;

    [SerializeField]
    private Resource m_health = new Resource();

    [SerializeField]
    private Resource m_mana = new Resource();

    [SerializeField]
    private int m_levelsAttained;

    [SerializeField]
    private CharacterStats m_leveledStats = new CharacterStats();

    [SerializeField]
    private Archetype m_equipedArchetype;

    [SerializeField]
    private List<Archetype> m_availableArchetypes = new List<Archetype>();

    public string ID => m_characterBase.ID;
    public ECharacterType CharacterType => m_characterType;
    public string Name => m_characterBase.Name;
    public Mesh Mesh => m_characterBase.Mesh;
    public Sprite Banner => m_characterBase.BannerIcon;
    public Sprite Profile => m_characterBase.ProfileIcon;
    public Sprite Arm => m_characterBase.ArmIcon;

    public int Level => m_characterBase.StartingLevel + m_levelsAttained;
    public int HP => m_characterBase.BaseStats.HP; //TODO: apply modifiers from equipment, level and End
    public int CurrentHP => m_health.Current;
    public float CurrentHPProportion => m_health.CurrentProportion;
    public int MP => m_characterBase.BaseStats.MP; //TODO: apply modifiers from equipment, level and Mag
    public int CurrentMP => m_mana.Current;
    public float CurrentMPProportion => m_mana.CurrentProportion;
    public int Str => m_characterBase.BaseStats.Strength; //TODO: apply modifiers from equipement and level
    public int Mag => m_characterBase.BaseStats.Magic; //TODO: apply modifiers from equipement and level
    public int End => m_characterBase.BaseStats.Endurance; //TODO: apply modifiers from equipement and level
    public int Agi => m_characterBase.BaseStats.Agility; //TODO: apply modifiers from equipement and level
    public int Luck => m_characterBase.BaseStats.Luck; //TODO: apply modifiers from equipement and level    

    public Character(CharacterSheet sheet)
    {
        m_characterBase = sheet;
        m_characterType = sheet.CharacterType;
        ApplyStats();

        m_health.OnResourceChange += HandleHealthChanged;
        m_mana.OnResourceChange += HandleManaChanged;
    }

    public void Dispose()
    {
        m_health.OnResourceChange -= OnHealthChanged;
        m_mana.OnResourceChange -= OnManaChanged;
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

    #region CharacterType Functions
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
    #endregion

    #region Health & Mana Functions
    public void ApplyHealth(int amount)
    {
        m_health.Apply(amount);
    }

    public void ApplyMana(int amount)
    {
        m_mana.Apply(amount);
    }

    private void HandleHealthChanged(int current, float proportion)
    {
        OnHealthChanged?.Invoke(current, proportion);
    }

    private void HandleManaChanged(int current, float proportion)
    {
        OnManaChanged?.Invoke(current, proportion);
    }
    #endregion

    private void ApplyStats()
    {
        m_health.SetMax(HP, true);
        m_mana.SetMax(MP, true);
    }
}
