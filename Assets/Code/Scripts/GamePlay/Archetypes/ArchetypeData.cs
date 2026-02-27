using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Archetype")]
public class ArchetypeData: UniqueScriptableObject
{
    [SerializeField]
    private string m_name;

    [SerializeField]
    private Sprite m_icon;

    [SerializeField]
    private Mesh m_mesh;

    public string Name => m_name;
    public Sprite Icon => m_icon;
    public Mesh Mesh => m_mesh;
}
