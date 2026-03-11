using UnityEngine;

[RequireComponent(typeof(LeftRightMover))]
public class LeftRightMoverBinder : PropertyBinder
{
    [SerializeField] private Condition[] m_rightConditions;
    [SerializeField] private Condition[] m_leftConditions;

    private LeftRightMover m_mover;

    private void Awake()
    {
        m_mover = GetComponent<LeftRightMover>();    
    }

    protected override void Apply(object value)
    {
        bool moveRight = true;
        bool moveLeft = true;

        foreach (Condition condition in m_rightConditions)
        {
            moveRight = condition.IsMet(value, out string message);

            if (message != null)
                Logger.LogError(message, m_logProfile);

            if (!moveRight)
                break;
        }

        foreach (Condition condition in m_leftConditions)
        {
            moveLeft = condition.IsMet(value, out string message);

            if (message != null)
                Logger.LogError(message, m_logProfile);

            if (!moveLeft)
                break;
        }

        if (moveLeft && moveRight)
        {
            Logger.LogError($"Both right and left conditions were met. Consider reworking the conditions. Setting to Left as fallback.", m_logProfile);
            moveLeft = true;
            moveRight = false;
        }

        if(!moveLeft && !moveRight)
        {
            Logger.LogError($"Neither right nor left conditions were met. Consider reworking the conditions. Setting to Left as fallback.", m_logProfile);
            moveLeft = true;
            moveRight = false;
        }

        if (moveRight)
        {
            m_mover.SetPosition(ECardinalPosition.Right);
            return;
        }

        m_mover.SetPosition(ECardinalPosition.Left);
    }
}
