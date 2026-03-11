using Mono.Cecil;
using System;
using System.Reflection;
using UnityEngine;

public abstract class PropertyBinder : MonoBehaviour, IBindableToProperty
{
    [SerializeField] 
    private string m_selectedProviderType;

    [SerializeField] 
    private string m_selectedSourceType;

    [SerializeField] 
    private string m_propertyKey;

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
            Debug.LogError($"{name}: No source assigned for binder.");
            return;
        }

        Type expectedType = ResolveProviderType();

        if (expectedType == null)
        {
            Debug.LogError($"{name}: Selected provider type '{m_selectedProviderType}' could not be resolved.");
            return;
        }

        if (!expectedType.IsInstanceOfType(provider))
        {
            Debug.LogError($"{name}: Provider type mismatch. Expected '{expectedType.FullName}', but received '{provider.GetType().FullName}'.");
            return;
        }

        if (!provider.TryGetRawProperty(m_propertyKey, out var raw))
        {
            Debug.LogError($"{name}: Could not bind property '{m_propertyKey}' from {provider.Name}.");
            return;
        }

        m_property = raw;

        var propType = raw.GetType();                    
        var genericArgs = propType.GetGenericArguments();
        if (genericArgs == null || genericArgs.Length != 1)
        {
            Debug.LogError($"{name}: Property '{m_propertyKey}' is not an ObservableProperty<T>.");
            return;
        }

        // Prepare Apply<TSource>
        m_sourceType = genericArgs[0];
        _closedApplyMethod = s_applyGenericMethod.MakeGenericMethod(m_sourceType);

        // Subscribe to OnChanged (Action<TSource>)
        var eventInfo = propType.GetEvent("OnChanged");
        if (eventInfo == null)
        {
            Debug.LogError($"{name}: Property '{m_propertyKey}' has no OnChanged event.");
            return;
        }

        var closedChangedMethod = s_onSourceChangedGenericMethod.MakeGenericMethod(m_sourceType);
        m_handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, closedChangedMethod);
        eventInfo.AddEventHandler(raw, m_handler);

        // Apply Initial value
        var valueProp = propType.GetProperty("Value");
        var initialValue = valueProp.GetValue(raw);
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
        Apply(value); // Calls the correct overload in the concrete binder
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