using UnityEngine;

public class EquipmentDisplay : MonoBehaviour
{
    private IBindableToProperty[] m_bindables;

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToProperty>();
    }

    private void OnDisable()
    {
        m_bindables.ForEach(x => x.UnBind());
    }

    public void BindToProperties(Equipment equipment)
    {
        m_bindables.ForEach(x => x.BindToProperty(equipment));
    }
}
