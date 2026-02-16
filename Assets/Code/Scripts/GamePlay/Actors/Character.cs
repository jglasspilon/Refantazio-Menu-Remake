using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character: IDisposable
{
    public event Action<ECharacterType> OnTypeChange;
    public event Action<bool> OnDeath;

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
    private Archetype m_equipedArchetype;

    [SerializeField]
    private List<Archetype> m_availableArchetypes = new List<Archetype>();

    private CharacterSheet m_characterBase;

    public string ID => m_characterBase.ID;
    public ECharacterType CharacterType => m_characterType;
    public string Name => m_characterBase.Name;
    public Mesh Mesh => m_characterBase.Mesh;
    public Sprite Banner => m_characterBase.BannerIcon;
    public Sprite Profile => m_characterBase.ProfileIcon;
    public Sprite Arm => m_characterBase.ArmIcon;
    public Resource HP => m_health;
    public Resource MP => m_mana;
    public CharacterStats Stats => m_stats;
    public bool IsDead => m_health.Current == 0 && m_health.Max > 0;

    #region Life Cycle Functions
    public Character(CharacterSheet sheet)
    {
        m_characterBase = sheet;
        m_name = sheet.Name;
        m_stats = sheet.Stats;
        m_characterType = sheet.CharacterType;
        ApplyStats();

        HP.OnEmpty += HandleOnHealthEmpty;
    }

    public void Dispose()
    {
        HP.OnEmpty -= HandleOnHealthEmpty;
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
    #endregion
}
