using System;
using UnityEngine;
using UnityEngine.Events;

public class SkillSelecter : UIObjectSelecter<MenuSkill>
{
    [SerializeField] private UnityEvent<Skill> OnSkillChanged;
    public event Action<Skill> OnSkillSelected;

    protected void OnEnable()
    {
        m_parentSection.OnConfirm.AddListener(SelectSkill);
    }

    private void OnDisable()
    {
        m_parentSection.OnConfirm.RemoveListener(SelectSkill);
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
