using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Follower: IPropertyProvider
{
    [SerializeField] private ObservableProperty<string> m_name = new ObservableProperty<string>();
    [SerializeField] private ObservableProperty<bool> m_isNew = new ObservableProperty<bool>();
    [SerializeField] private ObservableProperty<bool> m_isUnlocked = new ObservableProperty<bool>();
    [SerializeField] private ObservableProperty<Sprite> m_portrait = new ObservableProperty<Sprite>();
    [SerializeField] private Archetype m_relatedArchetye;
    [SerializeField] private Resource m_rank = new Resource(8);

    private string m_id;
    private int m_sortOrder;
    private Dictionary<string, IObservableProperty> m_properties;

    public string ID => m_id;
    public int SortOrder => m_sortOrder;
    public string Name => m_name.Value; 
    public bool IsNew => m_isNew.Value;
    public bool IsUnlocked => m_isUnlocked.Value;
    public Sprite Portrait => m_portrait.Value;
    public Archetype RelatedArchetype => m_relatedArchetye;
    public Resource Rank => m_rank;

    public Follower(FollowerData data)
    {
        m_id = data.ID;
        m_name.Value = data.Name;
        m_portrait.Value = data.Portrait;
        m_relatedArchetye = new Archetype(data.Archetype);
        m_sortOrder = data.SortOrder;
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

    public void Unlock()
    {
        m_isNew.Value = true;
        m_isUnlocked.Value = true;
        RankUp();
    }

    public void RankUp()
    {
        m_rank.Apply(1);
    }

    public void MarkAsSeen()
    {
        m_isNew.Value = false;
    }
}
