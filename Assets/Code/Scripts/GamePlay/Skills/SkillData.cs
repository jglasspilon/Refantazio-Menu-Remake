using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game Data/Skill")]
public class SkillData : UniqueScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private ESkillType m_skillType;
    [SerializeField] private Sprite m_icon;
    [TextArea]
    [SerializeField] private string m_description;
    [SerializeField] private int m_manaCost;
    [SerializeField] private ETargetingTypes m_targetingType;
    [SerializeField] private bool m_usableInBattle = true;
    [SerializeField] private bool m_usableInMenu = false;
    [SerializeField] private SkillEffect[] m_effects;

    private Dictionary<string, IObservableProperty> m_properties;

    public string Name => m_name;
    public ESkillType SkillType => m_skillType;
    public Sprite Icon => m_icon;
    public string Description => m_description;
    public int ManaCost => m_manaCost;
    public ETargetingTypes TargetingType => m_targetingType;
    public bool UsableInBattle => m_usableInBattle;
    public bool UsableInMenu => m_usableInMenu;
    public SkillEffect[] Effects => m_effects;

    public Skill CreateSkillFromData()
    {
        return new Skill(m_name, m_skillType, m_icon, m_description, m_manaCost, m_targetingType, m_usableInBattle, m_usableInMenu, m_effects);
    }
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
