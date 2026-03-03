using System;
using UnityEngine;

public class MenuSkill : PoolableObjectFromData<Skill>, ISelectable
{
    public event Action<bool> OnSetAsSelected;
    public event Action<bool> OnSetAsSelectable;

    [SerializeField]
    private bool m_potentialSelection;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private Skill m_skill;
    private IBindableToSkill[] m_bindables;

    public Skill Skill => m_skill;

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToSkill>();
    }

    public override void InitializeFromData(Skill skill)
    {
        if (skill == null)
        {
            Logger.LogError("Received an empty skill. Initializing an empty skill is not allowed.", m_logProfile);
            return;
        }

        transform.localScale = Vector3.one;
        m_skill = skill;
        SetAsSelectable(m_potentialSelection && m_skill.UsableInMenu);
        SetAsSelected(false);

        m_bindables.ForEach(x => x.BindToSkill(skill));
    }

    public override void ResetForPool()
    {
        SetAsSelected(false);
        m_skill = null;
        m_bindables.ForEach(x => x.Unbind());
    }   
    
    public void SetAsSelectable(bool selectable)
    {
        OnSetAsSelectable?.Invoke(selectable);
    }

    public void SetAsSelected(bool selected)
    {
        OnSetAsSelected?.Invoke(selected);
    }

    public void PauseSelection()
    {
        
    }
}
