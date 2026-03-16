using System;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour, ISelectable
{
    private Equipment m_equipment;

    public event Action<bool> OnSetAsSelected;
    public event Action<bool> OnSetAsSelectable;

    public Equipment Equipment => m_equipment;

    public void PauseSelection()
    {
        throw new NotImplementedException();
    }

    public void SetAsSelectable(bool selectable)
    {
        throw new NotImplementedException();
    }

    public void SetAsSelected(bool value)
    {
        throw new NotImplementedException();
    }
}
