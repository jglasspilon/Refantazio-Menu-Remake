using UnityEngine;

public class InventoryItem_Equipable : InventoryItem_Equipment
{
    [SerializeField] private GameObject m_equipedContent;

    private GearStatComparer_Equipment[] m_gearComparers;

    protected override void Awake()
    {
        base.Awake();
        m_gearComparers = GetComponentsInChildren<GearStatComparer_Equipment>();
    }

    public void SetAsEquiped(bool isEquiped)
    {
        m_equipedContent.SetActive(isEquiped);
    }

    public override void InitializeFromData(InventoryEntry entry)
    {
        base.InitializeFromData(entry);
        m_gearComparers.ForEach(x => x.InitializeWithItem(entry));
    }

    public void InitializeWithCharacter(Character character)
    {
        m_gearComparers.ForEach(x => x.InitializeWithCharacter(character));
    }
}
