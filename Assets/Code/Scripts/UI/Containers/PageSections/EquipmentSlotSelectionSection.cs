using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentSlotSelectionSection: PageSection
{
    [SerializeField] private EquipmentSlotSelecter m_selecter;
    
    public override UniTask EnterSection()
    {
        m_selecter.SelectCurrent();
        m_onEnter?.Invoke();
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selecter.SetApplicableToSelectable(x => false);
        m_selecter.UnselectAll();
        m_onExit?.Invoke();
        return default;
    }

    public override void ResetSection()
    {
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
    }
}
