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

    /// <summary>
    /// Gets or sets the fully qualified type name of the provider that this binder is expected to bind to. Editable only in the Unity Editor.
    /// Not meant to be used in runtime.
    /// </summary>
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

    /// <summary>
    /// Gets the resolved type of the selected provider, or null if the type string cannot be resolved.
    /// </summary>
    public Type ProviderType => Type.GetType(m_selectedProviderType);

    /// <summary>
    /// Gets or sets the fully qualified type name of the source property type expected by this binder. Editable only in the Unity Editor.
    /// Not meant to be used in runtime.
    /// </summary>
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

    /// <summary>
    /// Binds this binder to a property exposed by the given provider. Validates provider type compatibility, resolves the observable property, subscribes
    /// to its change event, and applies the initial value.
    /// </summary>
    public virtual void BindToProperty(IPropertyProvider provider)
    {
        if (provider == null)
        {
            Logger.LogError($"{name}: No Provider assigned for binder.", m_logProfile);
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

        // Unbind from old bindings before binding anew
        if (m_property != null)
            UnBind();

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

    /// <summary>
    /// Unsubscribes from the currently bound property (if any) and clears all cached binding state, ensuring the binder can be safely rebound.
    /// </summary>
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

    /// <summary>
    /// Resolves the provider type from the serialized type name. Returns null if the type string is empty or cannot be resolved.
    /// </summary>
    private Type ResolveProviderType()
    {
        if (string.IsNullOrEmpty(m_selectedProviderType))
            return null;

        return Type.GetType(m_selectedProviderType);
    }

    /// <summary>
    /// Traverses the inheritance chain of the given type to determine whether it represents an ObservableProperty, returning the contained value type
    /// if found.
    /// </summary>
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

    /// <summary>
    /// Handles strongly typed property change events by invoking the closed Apply(TSource) method associated with the current binding.
    /// </summary>
    private void OnSourceChangedGeneric<TSource>(TSource newValue)
    {
        _closedApplyMethod.Invoke(this, new object[] { newValue });
    }

    /// <summary>
    /// Handles dynamically typed property change events when the value type is not known at compile time. Invokes the closed Apply method using reflection.
    /// </summary>
    private void OnSourceChangedGenericDynamic(object newValue)
    {
        _closedApplyMethod.Invoke(this, new object[] { newValue });
    }

    /// <summary>
    /// Generic entry point for applying a new property value. Delegates to the best matching Apply overload implemented by the concrete binder.
    /// </summary>
    private void ApplyGeneric<TSource>(TSource value)
    {
        InvokeBestApplyOverload(value); 
    }

    /// <summary>
    /// Attempts to invoke the most specific Apply overload available for the provided value type. Falls back to Apply(object) if no exact match exists.
    /// </summary>
    private void InvokeBestApplyOverload(object value)
    {
        var valueType = value?.GetType() ?? typeof(object);

        // Find Apply(T) where T == valueType
        var method = GetType().GetMethod("Apply",BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,null,new[] { valueType },null);

        if (method != null)
        {
            method.Invoke(this, new[] { value });
            return;
        }

        // Fallback to Apply(object)
        Apply(value);
    }

    /// <summary>
    /// Called when no strongly typed Apply overload matches the incoming value. Concrete binders must implement this method to handle fallback cases or
    /// provide a generic application path.
    /// </summary>
    protected abstract void Apply(object value);
}