using System;
using System.Collections.Generic;

public sealed class ObjectResolver
{
    private static readonly Lazy<ObjectResolver> m_instance = new Lazy<ObjectResolver>(() => new ObjectResolver());   

    private readonly Dictionary<Type, object> m_registry = new Dictionary<Type, object>();
    private readonly Dictionary<Type, List<Action>> m_typedCallbacks = new Dictionary<Type, List<Action>>();

    private readonly object m_lock = new object();

    public static ObjectResolver Instance => m_instance.Value;

    public void Register<T>(T instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance), $"Cannot register a null instance for type {typeof(T).Name}");
        }

        lock (m_lock)
        {
            Type type = typeof(T);

            if (m_registry.ContainsKey(type))
            {
                throw new InvalidOperationException($"Type {type.Name} is already registered in the ObjectResolver.");
            }

            if (instance is IServiceWithLifecycle service)
            {
                service.Initialize();
            }

            m_registry[type] = instance;
            InvokeCallbacks(type);            
        }
    }

    public void RegisterOrReplace<T>(T instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(nameof(instance), $"Cannot register a null instance for type {typeof(T).Name}");
        }

        lock (m_lock)
        {
            Type type = typeof(T);
            if (m_registry.TryGetValue(type, out var existing) && existing is IServiceWithLifecycle existingService)
            {
                existingService.Shutdown();
            }           

            if (instance is IServiceWithLifecycle service)
            {
                service.Initialize();
            }

            m_registry[type] = instance;
            InvokeCallbacks(type);            
        }
    }

    public bool Unregister<T>()
    {
        var type = typeof(T);

        lock (m_lock)
        {
            if (m_registry.TryGetValue(type, out var existing) && existing is IServiceWithLifecycle existingService)
            {
                existingService.Shutdown();
                return m_registry.Remove(type);
            }
        }

        return false;
    }

    public T Resolve<T>()
    {
        Type type = typeof(T);

        lock (m_lock)
        {
            if (m_registry.TryGetValue(type, out var instance))
            {
                return (T)instance;
            }
        }

        throw new KeyNotFoundException($"No instance of type {type.Name} has been registered in the ObjectResolver.");
    }

    public bool TryResolve<T>(Action callback, out T instance)
    {
        Type type = typeof(T);

        lock (m_lock)
        {
            if (m_registry.TryGetValue(type, out object obj))
            {
                instance = (T)obj;
                return true;
            }
        }

        RegisterCallback(callback, type);
        instance = default;
        return false;
    }

    public void Clear()
    {
        lock (m_lock)
        {
            foreach(var existing in m_registry.Values)
            {
                if (existing is IServiceWithLifecycle service)
                {
                    service.Shutdown();
                }                
            }

            m_registry.Clear();
        }
    }

    private void RegisterCallback(Action callback, Type type)
    {
        if (callback == null)
        {
            return;
        }

        if (!m_typedCallbacks.TryGetValue(type, out List<Action> callbacks))
        {
            callbacks = new List<Action>();
            m_typedCallbacks[type] = callbacks;
        }

        if (!callbacks.Contains(callback))
            callbacks.Add(callback);
    }

    private void InvokeCallbacks(Type type)
    {
        if (m_typedCallbacks.TryGetValue(type, out List<Action> callbacks))
        {
            callbacks.RemoveAll(a => a is null);

            foreach (var callback in callbacks)
            {
                callback?.Invoke();
            }
        }
    }
}
