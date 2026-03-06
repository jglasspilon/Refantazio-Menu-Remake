using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MenuSkill : PoolableObjectFromData<Skill>, ISelectable
{
    public event Action<bool> OnSetAsSelected;
    public event Action<bool> OnSetAsSelectable;

    [SerializeField]
    private GameObject m_selectionSplotch, m_selectionFrame, m_shadow, m_selectedMpText;

    [SerializeField]
    private Animator m_anim;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private Skill m_skill;
    private Character m_character;
    private IBindableToSkill[] m_skillBindables;
    private IBindableToCharacter[] m_characterBindables;

    public Skill Skill => m_skill;

    private void Awake()
    {
        m_skillBindables = GetComponentsInChildren<IBindableToSkill>(true);
        m_characterBindables = GetComponentsInChildren<IBindableToCharacter>(true);
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
        SetAsSelected(false);
        m_skillBindables.ForEach(x => x.BindToSkill(skill));
    }

    public void InjectCharacter(Character character)
    {
        m_character = character;
        m_character.MP.OnResourceChange += HandleOnMpChanged;
        SetAsSelectable(m_skill.UsableInMenu && m_character.HasEnoughMana(m_skill.ManaCost));
        m_characterBindables.ForEach(x => x.BindToCharacter(character));
    }

    public override void ResetForPool()
    {
        m_character.MP.OnResourceChange -= HandleOnMpChanged;
        SetAsSelected(false);
        m_skill = null;
        m_character = null;
        m_skillBindables.ForEach(x => x.Unbind());
        m_characterBindables.ForEach(x => x.Unbind());
    }   
    
    public void SetAsSelectable(bool selectable)
    {
        OnSetAsSelectable?.Invoke(selectable);       
    }

    public void SetAsSelected(bool selected)
    {
        OnSetAsSelected?.Invoke(selected);
        m_anim.SetBool("IsSelected", selected);
        m_selectionFrame.SetActive(false);
        m_selectionSplotch.SetActive(selected);
        m_shadow.SetActive(!selected);
        m_selectedMpText.SetActive(selected);
    }

    public void PauseSelection()
    {
        m_selectionFrame.SetActive(true);
        m_selectionSplotch.SetActive(false);
    }

    private void HandleOnMpChanged(int current, float proportion, int delta)
    {
        SetAsSelectable(m_skill.UsableInMenu && current >= m_skill.ManaCost);
    }
}
