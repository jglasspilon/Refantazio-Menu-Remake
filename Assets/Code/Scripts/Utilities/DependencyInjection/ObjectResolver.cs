using System;
using System.Collections.Generic;

public sealed class ObjectResolver
{
    private static readonly Lazy<ObjectResolver> m_instance = new Lazy<ObjectResolver>(() => new ObjectResolver());   

    private readonly Dictionary<Type, object> m_registry = new Dictionary<Type, object>();
    private readonly Dictionary<Type, HashSet<CallbackEntry>> m_typedCallbacks = new Dictionary<Type, HashSet<CallbackEntry>>();

    private readonly object m_lock = new object();

    public static ObjectResolver Instance => m_instance.Value;

    private class CallbackEntry
    {
        public Action<object> Callback;
        public object Target;

        public bool IsDead()
        {
            if (Target == null)
                return true;

            if (Target is UnityEngine.Object unityObject)
                return unityObject == null;

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is not CallbackEntry other)
                return false;

            return Callback == other.Callback && Target == other.Target;
        }

        public override int GetHashCode()
        {
            int h1 = Callback?.GetHashCode() ?? 0;
            int h2 = Target?.GetHashCode() ?? 0;
            return h1 ^ h2;
        }
    }

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
            InvokeCallbacks<T>(instance);            
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
            InvokeCallbacks<T>(instance);            
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

    public bool TryResolve<T>(Action<T> callback, out T instance)
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

        RegisterCallback<T>(callback);
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

    private void RegisterCallback<T>(Action<T> callback)
    {
        if (callback == null)
            return;

        Type type = typeof(T);

        if (!m_typedCallbacks.TryGetValue(type, out var callbacks))
        {
            callbacks = new HashSet<CallbackEntry>();
            m_typedCallbacks[type] = callbacks;
        }
      
        Action<object> del = obj => callback((T)obj);

        var entry = new CallbackEntry
        {
            Callback = del,
            Target = callback.Target
        };       

        callbacks.Add(entry);
    }

    private void InvokeCallbacks<T>(T value)
    {
        Type type = typeof(T);
        if (m_typedCallbacks.TryGetValue(type, out var callbacks))
        {
            callbacks.RemoveWhere(x => x.IsDead());

            foreach (var callback in callbacks)
            {
                callback.Callback?.Invoke(value);
            }
        }
    }
}

