using UnityEngine;

[RequireComponent(typeof(LeftRightMover))]
public class DisplayCharacterPosition : MonoBehaviour, IBindableToCharacter
{
    [SerializeField]
    private GameObject m_frontContent, m_backContent;

    private LeftRightMover m_positionMover;
    private Character m_character;

    private void Awake()
    {
        m_positionMover = GetComponent<LeftRightMover>();
    }

    public void BindToCharacter(Character character)
    {
        if(character == null) 
            return;

        m_character = character;
        m_positionMover.SetEnable(!m_character.IsGuide);
        Display(m_character.BattlePosition.Value);
    }

    public void Unbind()
    {
        if (m_character == null)
            return;

        m_character = null;
    }

    private void Display(EBattlePosition position)
    {
        ECardinalPosition targetPosition = position == EBattlePosition.Back ? ECardinalPosition.Right : ECardinalPosition.Left;

        m_positionMover.SetPosition(targetPosition);
        m_frontContent.SetActive(position == EBattlePosition.Front);
        m_backContent.SetActive(position == EBattlePosition.Back);
    }
}
