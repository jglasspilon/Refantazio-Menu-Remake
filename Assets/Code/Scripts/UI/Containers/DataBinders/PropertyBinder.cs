using System;
using System.Reflection;
using UnityEngine;

public abstract class PropertyBinder : MonoBehaviour, IBindableToProperty
{
    [SerializeField] private string m_selectedProviderType;
    [SerializeField] private string m_selectedSourceType;
    [SerializeField] private string m_propertyKey;
    [SerializeField] protected LoggingProfile m_logProfile;

    private object m_property;
    private Type m_sourceType;
    private Delegate m_handler;

    // Cached generic methods
    private static readonly MethodInfo s_onSourceChangedGenericMethod = typeof(PropertyBinder).GetMethod(nameof(OnSourceChangedGeneric), BindingFlags.NonPublic | BindingFlags.Instance);
    private static readonly MethodInfo s_applyGenericMethod = typeof(PropertyBinder).GetMethod(nameof(ApplyGeneric), BindingFlags.NonPublic | BindingFlags.Instance);
    private MethodInfo _closedApplyMethod;

    public string SelectedProviderType
    {
        get => m_selectedProviderType;

        set 
        {
            #if UNITY_EDITOR
                m_selectedProviderType = value;
            #else
                Debug.LogWarning("SelectedProviderType should not be modified at runtime.");
            #endif
        }

    }

    public Type ProviderType => Type.GetType(m_selectedProviderType);

    public string SelectedSourceType
    {
        get => m_selectedSourceType;

        set 
        {
            #if UNITY_EDITOR
                m_selectedSourceType = value;
            #else
                Debug.LogWarning("SelectedSourceType should not be modified at runtime.");
            #endif
        }
    }

    public virtual void BindToProperty(IPropertyProvider provider)
    {
        if (provider == null)
        {
            Logger.LogError($"{name}: No source assigned for binder.", m_logProfile);
            return;
        }

        Type expectedType = ResolveProviderType();

        if (expectedType == null)
        {
            Logger.LogError($"{name}: Selected provider type '{m_selectedProviderType}' could not be resolved.", m_logProfile);
            return;
        }

        if (!expectedType.IsInstanceOfType(provider))
        {
            Logger.LogError($"{name}: Provider type mismatch. Expected '{expectedType.FullName}', but received '{provider.GetType().FullName}'.", m_logProfile);
            return;
        }

        if (!provider.TryGetPropertyRaw(m_propertyKey, out object raw))
        {
            Logger.LogError($"{name}: Could not bind property '{m_propertyKey}' from {provider.Name}.", m_logProfile);
            return;
        }

        Type propType = raw.GetType();                    
        Type valueType = GetObservablePropertyValueType(propType);

        if (valueType == null )
        {
            Logger.LogError($"{name}: Property '{m_propertyKey}' is not an ObservableProperty<T>.", m_logProfile);
            return;
        }

        // Prepare Apply<TSource>
        m_property = raw;     
        m_sourceType = valueType;
        _closedApplyMethod = s_applyGenericMethod.MakeGenericMethod(m_sourceType);

        // Subscribe to OnChanged (Action<TSource>)
        EventInfo eventInfo = propType.GetEvent("OnChanged");
        if (eventInfo == null)
        {
            Logger.LogError($"{name}: Property '{m_propertyKey}' has no OnChanged event.", m_logProfile);
            return;
        }

        // Bind the property's OnChanged<T> event to this binder's typed Apply<T> method.
        MethodInfo closedChangedMethod = s_onSourceChangedGenericMethod.MakeGenericMethod(m_sourceType);
        m_handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, closedChangedMethod);
        eventInfo.AddEventHandler(raw, m_handler);

        // Apply Initial value
        PropertyInfo valueProp = propType.GetProperty("Value");
        object initialValue = valueProp.GetValue(raw);
        OnSourceChangedGenericDynamic(initialValue);
    }

    public virtual void UnBind()
    {
        if (m_property == null)
            return;

        var eventInfo = m_property.GetType().GetEvent("OnChanged");
        if (eventInfo != null && m_handler != null)
            eventInfo.RemoveEventHandler(m_property, m_handler);

        m_property = null;
        m_handler = null;
        m_sourceType = null;
        _closedApplyMethod = null;
    }

    private Type ResolveProviderType()
    {
        if (string.IsNullOrEmpty(m_selectedProviderType))
            return null;

        return Type.GetType(m_selectedProviderType);
    }

    // Recursively walk through inheritance chain to find Observable Property type
    private Type GetObservablePropertyValueType(Type type)
    {
        while (type != null)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ObservableProperty<>))
            {
                return type.GetGenericArguments()[0];
            }

            type = type.BaseType;
        }
        return null;
    }

    // Called by Action<TSource>
    private void OnSourceChangedGeneric<TSource>(TSource newValue)
    {
        // Call Apply<TSource>(TSource)
        _closedApplyMethod.Invoke(this, new object[] { newValue });
    }

    // Used for initial value forwarding
    private void OnSourceChangedGenericDynamic(object newValue)
    {
        _closedApplyMethod.Invoke(this, new object[] { newValue });
    }

    // This is the generic Apply<TSource> that concrete binders override
    private void ApplyGeneric<TSource>(TSource value)
    {
        InvokeBestApplyOverload(value); 
    }

    // Calls the correct overload in the concrete binder
    private void InvokeBestApplyOverload(object value)
    {
        var valueType = value?.GetType() ?? typeof(object);

        // Find Apply(T) where T == valueType
        var method = GetType().GetMethod(
            "Apply",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
            null,
            new[] { valueType },
            null
        );

        if (method != null)
        {
            method.Invoke(this, new[] { value });
            return;
        }

        // Fallback to Apply(object)
        Apply(value);
    }

    /// <summary>
    /// Concrete binders implement strongly-typed Apply(TSource) overloads.
    /// Example:
    ///     protected void Apply(int value) { ... }
    ///     protected void Apply(float value) { ... }
    ///     protected void Apply(MyEnum value) { ... }
    /// </summary>
    protected abstract void Apply(object value);
}