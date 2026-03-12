using System.Collections.Generic;
using UnityEngine;

public class Skill: IPropertyProvider
{
    [SerializeField] private ObservableProperty<string> m_name = new ObservableProperty<string>();
    [SerializeField] private ObservableProperty<ESkillType> m_skillType = new ObservableProperty<ESkillType>();
    [SerializeField] private ObservableProperty<Sprite> m_icon = new ObservableProperty<Sprite>();
    [SerializeField] private ObservableProperty<string> m_description = new ObservableProperty<string>();
    [SerializeField] private ObservableProperty<int> m_manaCost = new ObservableProperty<int>();
    [SerializeField] private ObservableProperty<bool> m_usableInBattle = new ObservableProperty<bool>();
    [SerializeField] private ObservableProperty<bool> m_usableInMenu = new ObservableProperty<bool>();
    [SerializeField] private ETargetingTypes m_targetingType;
    [SerializeField] private SkillEffect[] m_effects;

    private Dictionary<string, IObservableProperty> m_properties;

    public string Name => m_name.Value;
    public ESkillType SkillType => m_skillType.Value;
    public Sprite Icon => m_icon.Value;
    public string Description => m_description.Value;
    public int ManaCost => m_manaCost.Value;
    public ETargetingTypes TargetingType => m_targetingType;
    public bool UsableInBattle => m_usableInBattle.Value;
    public bool UsableInMenu => m_usableInMenu.Value;
    public SkillEffect[] Effects => m_effects;

    public Skill(string name, ESkillType skillType, Sprite icon, string description, int manaCost, ETargetingTypes targetingType, bool usableInBattle, bool usableInMenu, SkillEffect[] effects)
    {
        m_name.Value = name;
        m_skillType.Value = skillType;
        m_icon.Value = icon;
        m_description.Value = description;
        m_manaCost.Value = manaCost;        
        m_usableInBattle.Value = usableInBattle;
        m_usableInMenu.Value = usableInMenu;
        m_targetingType = targetingType;
        m_effects = effects;
        InitializeProperties();
    }

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
}
