using System;
using UnityEngine;

public class SkillSelecter : UIObjectSelecter<MenuSkill>
{
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
}
