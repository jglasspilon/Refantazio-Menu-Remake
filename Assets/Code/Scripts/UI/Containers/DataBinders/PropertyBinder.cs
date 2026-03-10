using UnityEngine;

public abstract class PropertyBinder<T> : MonoBehaviour, IBindableToProperty
{
    [SerializeField] 
    private string m_propertyKey;

    private IPropertyProvider m_source;
    private ObservableProperty<T> m_property;

    public virtual void BindToProperty(IPropertyProvider provier)
    {
        if (provier == null)
        {
            Debug.LogError($"{name}: No source assigned for binder.");
            return;
        }

        m_source = provier;

        if (!m_source.TryGetProperty(m_propertyKey, out m_property))
        {
            Debug.LogError($"{name}: Could not bind property '{m_propertyKey}' from {m_source.Name} of type {m_source.GetType()}.");
            return;
        }

        m_property.OnChanged += HandleValueChanged;
        HandleValueChanged(m_property.Value);
    }

    public virtual void UnBind()
    {
        if (m_property == null)
            return;
        
        m_property.OnChanged -= HandleValueChanged;
        m_property = null;
        m_source = null;
    }

    private void HandleValueChanged(T newValue)
    {
        Apply(newValue);
    }

    protected abstract void Apply(T value);
}