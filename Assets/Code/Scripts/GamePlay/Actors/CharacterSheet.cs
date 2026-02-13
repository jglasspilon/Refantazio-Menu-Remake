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
    private int m_startingLevel;

    [SerializeField]
    private ArchetypeData m_startingArchetype;

    [SerializeField]
    private CharacterStats m_baseStats;

    [SerializeField]
    private Skill[] m_startingSkills;

    [SerializeField]
    private Sprite m_profileIcon, m_bannerIcon, m_armIcon;

    [SerializeField]
    private Mesh m_mesh;

    public ECharacterType CharacterType => m_characterType;
    public string Name => m_name;
    public int StartingLevel => m_startingLevel;
    public ArchetypeData StartingArchetype => m_startingArchetype;
    public CharacterStats BaseStats => m_baseStats;
    public Skill[] StartingSkills => m_startingSkills;
    public Sprite ProfileIcon => m_profileIcon;
    public Sprite BannerIcon => m_bannerIcon;
    public Sprite ArmIcon => m_armIcon;
    public Mesh Mesh => m_mesh;
}

[Serializable]
public struct CharacterStats
{
    public int HP;
    public int MP;
    public int Strength;
    public int Magic;
    public int Endurance;
    public int Agility;
    public int Luck;
}

public enum ECharacterType
{
    Leader,
    Reserve,
    Party,
    Guide,
    Enemy
}
