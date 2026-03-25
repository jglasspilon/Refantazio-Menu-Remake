using UnityEngine;

public class EquipmentStatComparerContainer : MonoBehaviour
{
    [SerializeField] private GameObject m_equipedCharacterContent, m_prospectiveCharacterContent;

    private IBindableToProperty[] m_bindableToEquiped, m_bindableToProspective;
    private Character m_selectedCharacter, m_simulatedCharacter;
    private EEquipmentSlotType m_equipmentChangeType;

    private void Awake()
    {
        m_bindableToEquiped = m_equipedCharacterContent.GetComponentsInChildren<IBindableToProperty>();
        m_bindableToProspective = m_prospectiveCharacterContent.GetComponentsInChildren<IBindableToProperty>();
    }

    private void OnDisable()
    {
        ResetComparer();
    }

    public void ResetComparer()
    {
        m_bindableToEquiped.ForEach(x => x.UnBind());
        m_bindableToProspective.ForEach(x => x.UnBind());
    }

    public void HandleOnCharacterChanged(Character character)
    {
        if(character == null) 
            return;

        if (m_selectedCharacter != null)
            m_selectedCharacter.Equipment.OnEquipmentChanged -= HandleOnEquipmentChanged;

        m_selectedCharacter = character;
        m_selectedCharacter.Equipment.OnEquipmentChanged += HandleOnEquipmentChanged;
        m_simulatedCharacter = m_selectedCharacter.CreateSimulatedCharacter();

        m_bindableToEquiped.ForEach(x => x.BindToProperty(m_selectedCharacter));
        m_bindableToProspective.ForEach(x => x.BindToProperty(m_simulatedCharacter));
    }

    public void HandleOnEquipmentSlotChanged(EEquipmentSlotType slotType)
    {
        m_equipmentChangeType = slotType;
    }

    public void HandleOnArchetypeChanged(Archetype archetype)
    {
        if (m_simulatedCharacter == null || archetype == null)
            return;

        m_simulatedCharacter.Equipment.EquipArchetype(archetype);
        m_bindableToProspective.ForEach(x => x.BindToProperty(m_simulatedCharacter));
    }

    private void HandleOnEquipmentChanged()
    {
        m_bindableToEquiped.ForEach(x => x.BindToProperty(m_selectedCharacter));
    }
}
