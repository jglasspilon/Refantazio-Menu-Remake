using System;
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
}

[Serializable]
public class CharacterStats
{
    public event Action<Stat> OnStatChange;

    public Stat Level = new Stat();
    public Stat HP = new Stat();
    public Stat MP = new Stat();
    public Stat Strength = new Stat();
    public Stat Magic = new Stat();
    public Stat Endurance = new Stat();
    public Stat Agility = new Stat();
    public Stat Luck = new Stat();

    public CharacterStats()
    {
        Level.OnValueChange += OnStatChange;
        HP.OnValueChange += OnStatChange;
        MP.OnValueChange += OnStatChange;
        Strength.OnValueChange += OnStatChange;
        Magic.OnValueChange += OnStatChange;
        Endurance.OnValueChange += OnStatChange;
        Agility.OnValueChange += OnStatChange;
        Luck.OnValueChange += OnStatChange;
    }

    private void HandleOnStatChange(Stat statChange)
    {
        OnStatChange?.Invoke(statChange);
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
