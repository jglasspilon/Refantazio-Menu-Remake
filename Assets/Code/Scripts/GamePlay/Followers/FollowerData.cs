using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Follower")]
public class FollowerData: UniqueScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private Sprite m_portrait;
    [SerializeField] private ArchetypeData m_relatedArchetye;
    [SerializeField] private int m_sortOrder;

    public string Name => m_name;
    public Sprite Portrait => m_portrait;
    public ArchetypeData Archetype => m_relatedArchetye;
    public int SortOrder => m_sortOrder;
}
