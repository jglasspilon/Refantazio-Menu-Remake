using UnityEngine;

public class ComponentEnablerBinder : PropertyBinder
{
    [Header("Conditional Binding:")]
    [SerializeField] private MonoBehaviour m_componentToEnable;
    [SerializeField] private Condition[] m_conditionsToMeet;

    protected override void Apply(object value)
    {
        if(m_componentToEnable == null)
        {
            Logger.Log($"Failed to enable component from property for {gameObject.name}. No component to enable set.", m_logProfile);
            return;
        }

        bool enable = true;

        foreach (Condition condition in m_conditionsToMeet)
        {
            enable = condition.IsMet(value, out string message);

            if (message != null)
                Logger.LogError(message, m_logProfile);

            if (!enable)
                break;
        }

        m_componentToEnable.enabled = false;
        m_componentToEnable.enabled = enable;
    }
}
