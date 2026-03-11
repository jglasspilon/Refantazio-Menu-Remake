using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplaySkillCost : MonoBehaviour, IBindableToSkill
{
    private TextMeshProUGUI m_text;
    private Skill m_skill;

    private void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
    }

    public void BindToSkill(Skill skill)
    {
        if (skill == null)
            return;

        m_skill = skill;
        Display(m_skill.ManaCost);
    }

    public void Unbind()
    {
        m_skill = null;
    }

    private void Display(int cost)
    {
        m_text.text = Helper.StringFormatting.FormatIntForUI(cost, 3, 0.06f);
    }
}
