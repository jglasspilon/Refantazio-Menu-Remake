using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Equip Effects")]
public class EquipEffect : ScriptableObject
{
    [SerializeField][TextArea]
    private string m_description;

    public string Description => m_description;
}
