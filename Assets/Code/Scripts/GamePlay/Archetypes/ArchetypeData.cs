using UnityEngine;

[CreateAssetMenu(fileName = "Archetype", menuName = "Game Data")]
public class ArchetypeData: UniqueScriptableObject
{
    [SerializeField]
    private string m_name;

    [SerializeField]
    private Mesh m_mesh;
}
