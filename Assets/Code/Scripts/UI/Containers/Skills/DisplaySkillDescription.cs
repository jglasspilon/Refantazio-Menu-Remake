using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplaySkillDescription : MonoBehaviour, IBindableToSkill
{
    private TextMeshProUGUI m_text;
    private Skill m_skill;

    private void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
    }

    public void BindToSkill(Skill skill)
    {
        if(skill == null) 
            return;

        m_skill = skill;
        Display(m_skill.Description);
    }

    public void Unbind()
    {
        m_skill = null;
    }

    private void Display(string description)
    {
        m_text.text = description;
    }
}
