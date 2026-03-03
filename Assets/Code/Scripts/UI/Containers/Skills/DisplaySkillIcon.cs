using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DisplaySkillIcon : MonoBehaviour, IBindableToSkill
{
    private Image m_icon;
    private Skill m_skill;

    private void Awake()
    {
        m_icon = GetComponent<Image>();
    }

    public void BindToSkill(Skill skill)
    {
        if (skill == null)
            return;

        m_skill = skill;
        Display(m_skill.Icon);
    }

    public void Unbind()
    {
        m_skill = null;
    }

    private void Display(Sprite icon)
    {
        m_icon.enabled = icon != null;
        m_icon.sprite = icon;
    }
}
