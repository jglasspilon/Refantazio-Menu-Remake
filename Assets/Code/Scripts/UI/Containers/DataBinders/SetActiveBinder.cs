using System.Linq;
using UnityEngine;

public class SetActiveBinder : PropertyBinder
{
    [Header("Conditional Binding:")]
    [SerializeField] private Condition[] m_conditionsToMeet;

    protected override void Apply(object value)
    {
        bool show = true;

        foreach(Condition condition in m_conditionsToMeet)
        {
            show = condition.IsMet(value, out string message);

            if (message != null)
                Logger.LogError(message, m_logProfile);

            if (!show)
                break;
        }

        gameObject.SetActive(show);
    }
}
