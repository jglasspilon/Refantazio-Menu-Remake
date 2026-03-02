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
    private bool m_battleOnly = true;

    [SerializeField]
    private SkillEffect[] m_effects;
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
