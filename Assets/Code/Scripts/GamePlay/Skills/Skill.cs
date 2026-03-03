using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Skill")]
public class Skill : UniqueScriptableObject
{
    [SerializeField]
    private string m_name;

    [SerializeField]
    private ESkillType m_skillType;

    [SerializeField]
    private Sprite m_icon;

    [SerializeField][TextArea]
    private string m_description;

    [SerializeField]
    private int m_manaCost;

    [SerializeField]
    private ETargetingTypes m_targetingType;

    [SerializeField]
    private bool m_usableInBattle = true;

    [SerializeField]
    private bool m_usableInMenu = false;

    [SerializeField]
    private SkillEffect[] m_effects;

    public string Name => m_name;
    public ESkillType SkillType => m_skillType;
    public Sprite Icon => m_icon;
    public string Description => m_description;
    public int ManaCost => m_manaCost;
    public ETargetingTypes TargetingType => m_targetingType;
    public bool UsableInBattle => m_usableInBattle;
    public bool UsableInMenu => m_usableInMenu;
    public SkillEffect[] Effects => m_effects;
}

public enum ESkillType
{
    Recovery,
    Support,
    Slash, 
    Pierce,
    Strike,
    Fire, 
    Ice,
    Electric,
    Wind,
    Light,
    Dark,
    HeroPassive,
    Passive,
    Almighty,
    Ailment
}
