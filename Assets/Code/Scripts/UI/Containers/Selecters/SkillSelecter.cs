using System;
using UnityEngine;
using UnityEngine.Events;

public class SkillSelecter : UIObjectSelecter<MenuSkill>
{
    [SerializeField] private UnityEvent<Skill> OnSkillChanged;
    public event Action<Skill> OnSkillSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += SelectSkill;
    }

    public void SelectSkill()
    {
        OnSkillSelected?.Invoke(SelectedObject.Skill);
    }

    protected override void VirtualInvoke()
    {
        OnSkillChanged?.Invoke(SelectedObject.Skill);
    }
}
