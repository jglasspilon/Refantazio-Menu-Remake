using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class Character: IPropertyProvider
{
    [SerializeField] private CharacterStats m_stats;
    [SerializeField] private Level m_level;
    [SerializeField] private Resource m_health = new Resource(0);
    [SerializeField] private Resource m_mana = new Resource(0);
    [SerializeField] private ObservableProperty<string> m_name = new ObservableProperty<string>();
    [SerializeField] private ObservableProperty<ECharacterType> m_characterType = new ObservableProperty<ECharacterType>();
    [SerializeField] private ObservableProperty<EBattlePosition> m_battlePosition = new ObservableProperty<EBattlePosition>();
    [SerializeField] private ObservableProperty<bool> m_isDead = new ObservableProperty<bool>();
    [SerializeField] private ObservableProperty<Sprite> m_banner = new ObservableProperty<Sprite>();
    [SerializeField] private EquipmentExecutor m_equipment;   
    [SerializeField] private List<Archetype> m_availableArchetypes = new List<Archetype>();

    private CharacterSheet m_characterBase;
    private Dictionary<string, IObservableProperty> m_properties;
    private bool m_hpCalculated, m_mpCalculated;
    public string ID => m_characterBase.ID;
    public bool IsValid => m_characterBase != null;
    public string Name => m_name.Value;
    public Mesh Mesh => m_characterBase.Mesh;
    public Sprite Profile => m_characterBase.ProfileIcon;
    public Sprite Arm => m_characterBase.ArmIcon;
    public Level Level => m_level;
    public Resource HP => m_health;
    public Resource MP => m_mana;
    public CharacterStats Stats => m_stats;
    public EquipmentExecutor Equipment => m_equipment;
    public ObservableProperty<bool> IsDead => m_isDead;
    public ObservableProperty<EBattlePosition> BattlePosition => m_battlePosition;
    public ObservableProperty<ECharacterType> CharacterType => m_characterType;
    public Skill[] Skills => m_equipment?.Archetype?.GetAvailableSkills() ?? Array.Empty<Skill>();   
    public bool IsLeader => m_characterType.Value == ECharacterType.Leader;
    public bool IsGuide => m_characterType.Value == ECharacterType.Guide;
    public bool IsInActiveParty => m_characterType.Value == ECharacterType.Party || IsLeader;

    #region Life Cycle Functions
    public Character(CharacterSheet sheet)
    {
        Archetype startArchetype = null;
        m_characterBase = sheet;
        m_name.Value = sheet.Name;
        m_stats = new CharacterStats(sheet.HP, sheet.MP, sheet.Str, sheet.Mag, sheet.End, sheet.Agi, sheet.Luck);
        m_level = sheet.CreateLevel();
        m_characterType.Value = sheet.CharacterType;
        m_banner.Value = sheet.BannerIcon;
        m_battlePosition.Value = m_characterType.Value == ECharacterType.Guide ? EBattlePosition.Undetermined : EBattlePosition.Front;

        if (m_characterBase.StartingArchetype != null)
        {
            startArchetype = new Archetype(m_characterBase.StartingArchetype);

            if(!m_availableArchetypes.Contains(startArchetype)) 
                m_availableArchetypes.Add(startArchetype);
        }

        Equipment startingWeapon = sheet.StartingWeapon == null ? null : sheet.StartingWeapon.CreateItemFromData() as Equipment;
        Equipment startingArmor = sheet.StartingArmor == null ? null : sheet.StartingArmor.CreateItemFromData() as Equipment;
        Equipment startingGear = sheet.StartingGear == null ? null : sheet.StartingGear.CreateItemFromData() as Equipment;
        Equipment startingAccessory = sheet.StartingAccessory == null ? null : sheet.StartingAccessory.CreateItemFromData() as Equipment;
        m_equipment = new EquipmentExecutor(startingWeapon, startingArmor, startingGear, startingAccessory, startArchetype, this);

        ApplyHp(Stats.Endurance.Value);
        ApplyMp(Stats.Magic.Value);

        m_health.OnEmpty += HandleOnHealthEmpty;
        Level.OnChanged += ApplyHp;
        Level.OnChanged += ApplyMp;
        Stats.Endurance.OnChanged += ApplyHp;
        Stats.Magic.OnChanged += ApplyMp;
        Equipment.OnEquipmentChanged += InitializeProperties;
        InitializeProperties();
    }
    #endregion

    #region Property Provision Functions
    private void InitializeProperties()
    {
        m_properties = Helper.DataHandling.BuildPropertyMap(this);
    }

    public bool TryGetPropertyRaw(string key, out object value)
    {
        if (m_properties.TryGetValue(key, out IObservableProperty raw))
        {
            value = raw;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetProperty<T>(string key, out ObservableProperty<T> value)
    {
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
        m_characterType.Value = ECharacterType.Leader;
    }

    public void SetCharacterToActiveParty()
    {
        m_characterType.Value = ECharacterType.Party;
    }

    public void RemoveCharacterFromActiveParty()
    {
        m_characterType.Value = m_characterBase.CharacterType;
    }

    public void SetCharacterBattlePosition(EBattlePosition battlePosition)
    {
        if (battlePosition == m_battlePosition.Value || m_characterType.Value == ECharacterType.Guide)
            return;

        m_battlePosition.Value = battlePosition;
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

    private void HandleOnHealthEmpty(bool isEmpty)
    {
        if (m_health.Max > 0)
        {
            m_isDead.Value = isEmpty;
        }
    }
    #endregion

    #region Stats Functions
    private void ApplyHp(int newValue)
    {
        EResourceSetProcedure prc = m_hpCalculated ? EResourceSetProcedure.Keep : EResourceSetProcedure.Fill;
        int value = Helper.GameMath.GetMaxHpFromEndurance(Stats.HP.Value, Stats.Endurance.Value, Level.Value);
        m_health.SetMax(value, prc);
        m_hpCalculated = true;
    }

    private void ApplyMp(int newValue)
    {
        EResourceSetProcedure prc = m_mpCalculated ? EResourceSetProcedure.Keep : EResourceSetProcedure.Fill;
        int value = Helper.GameMath.GetMapMpFromMagic(Stats.MP.Value, Stats.Magic.Value, Level.Value);
        m_mana.SetMax(value, prc);
        m_mpCalculated = true;
    }
    #endregion
}
