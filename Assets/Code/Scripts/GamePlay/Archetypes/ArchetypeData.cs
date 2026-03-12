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

    [SerializeField]
    private EEquipmentType m_equipableWeaponType;

    [SerializeField]
    private Equipment m_defaultWeapon;

    [SerializeField]
    private SerializedKeyValuePair<int, SkillData[]>[] m_skillsByRank;

    [SerializeField]
    private AnimationCurveAsset m_rankExpCurve;

    [SerializeField]
    private SerializedKeyValuePair<EStatType, AnimationCurve>[] m_rankedStatCurves;

    public string Name => m_name;
    public Sprite Icon => m_icon;
    public Mesh Mesh => m_mesh;
    public EEquipmentType EquipableWeaponType => m_equipableWeaponType;
    public Equipment DefaultWeapon => m_defaultWeapon;
    public SerializedKeyValuePair<int, SkillData[]>[] SkillsByRank => m_skillsByRank;
    public SerializedKeyValuePair<EStatType, AnimationCurve>[] RankedStatCurves => m_rankedStatCurves;
    public AnimationCurveAsset RankExpCurve => m_rankExpCurve;
}
