using System;
using UnityEngine;

public class EquipmentSlot : SelectableSlot
{
    [SerializeField] private Animator m_anim;

    public override void SetAsSelected(bool selected)
    {
        base.SetAsSelected(selected);
        m_anim.SetBool("IsSelected", selected);
    }
}
