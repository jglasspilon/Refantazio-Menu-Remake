using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class SetColorBasedOnSkillAffordability : MonoBehaviour, IBindableToSkill, IBindableToCharacter
{
    [SerializeField]
    private Color m_canAffordColor, m_cannotAffordColor;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private MaskableGraphic m_graphic;
    private Skill m_skill;
    private Character m_character;

    private void Awake()
    {
        m_graphic = GetComponent<MaskableGraphic>();
    }

    public void BindToCharacter(Character character)
    {
        if (character == null)
            return;

        m_character = character;
        m_character.MP.OnResourceChange += DisplayMpAffordability;
        DisplayMpAffordability(m_character.MP, 0);
    }

    public void BindToSkill(Skill skill)
    {
        if (skill == null)
            return;

        m_skill = skill;
    }

    public void Unbind()
    {
        if(m_character != null)
        {
            m_character.MP.OnResourceChange -= DisplayMpAffordability;
        }

        m_skill = null;
        m_character = null;
    }

    private void DisplayMpAffordability(Resource resource, int delta)
    {
        if(m_skill == null)
        {
            Logger.LogError("No skill attached, cannot display mp affordability", m_logProfile);
            return;
        }

        if (!m_skill.UsableInMenu)
            return;

        bool canAfford = resource.Current >= m_skill.ManaCost;
        m_graphic.color = canAfford ? m_canAffordColor : m_cannotAffordColor;
    }
}
