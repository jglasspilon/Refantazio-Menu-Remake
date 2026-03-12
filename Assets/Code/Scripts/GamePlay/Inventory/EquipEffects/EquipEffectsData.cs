using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Equip Effects")]
public class EquipEffectData: ScriptableObject
{
    [TextArea]
    [SerializeField] private string m_description;

    public string Description => m_description;

    public EquipEffect CreateEquipEffectFromData()
    {
        return new EquipEffect(m_description);
    }
}

public class EquipEffect: ISubPropertyProvider
{
    [SerializeField] private ObservableProperty<string> m_description = new ObservableProperty<string>();

    public string Description => m_description.Value;

    public EquipEffect(string description)
    {
        m_description.Value = description;
    }

    public IEnumerable<KeyValuePair<string, IObservableProperty>> GetSubProperties(string parentKey)
    {
        return Helper.DataHandling.GetObservableFields(this, parentKey);
    }
}