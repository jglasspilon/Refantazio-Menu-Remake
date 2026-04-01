using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIObjectGeneraterFromPool<T, TData> : MonoBehaviour, IGenerator where T : PoolableObjectFromData<TData>
{
    [SerializeField] protected Transform m_holder;
    [SerializeField] protected LoggingProfile m_logProfile;

    protected AssetPoolManager m_assetPool;
    protected List<T> m_content = new List<T>();  
    protected GameObject m_owner;
    protected bool m_isInitialized;

    /// <summary>
    /// Event invoked whenever content is generated.>.
    /// </summary>
    public event Action OnGenerate;

    /// <summary>
    /// Indicates whether the generator has been successfully initialized with an AssetPoolManager and an owning GameObject.
    /// </summary>
    public bool IsInitialized => m_isInitialized;

    public UIObjectGeneraterFromPool(){ }

    /// <summary>
    /// Initializes the generator with a reference to the AssetPoolManager and the owning GameObject. Must be called before generating content.
    /// </summary>
    public void Initialize(AssetPoolManager assetPool, GameObject owner)
    {
        m_assetPool = assetPool;
        m_owner = owner;
        m_isInitialized = true;
    }

    /// <summary>
    /// Generates UI objects from the provided data set using pooled instances. Clears any previously generated content, creates new items from the pool,
    /// initializes them with the supplied data, and returns the generated objects.
    /// </summary>
    public T[] GenerateContent(TData[] itemsToGenerate)
    {
        if (m_assetPool == null)
        {
            Logger.LogError($"Could not generate UI content for {m_owner.name}. No asset pool found.", m_logProfile);
            return Array.Empty<T>(); ;
        }

        ClearGeneratedContent();
        foreach (TData item in itemsToGenerate)
        {
            GeneratePoolableFromData(item);
        }            

        OnGenerate?.Invoke();
        return m_content.ToArray();
    }

    /// <summary>
    /// Returns all currently generated UI objects back to the pool and clears the internal content list. Safe to call even when no content exists.
    /// </summary>
    public void ClearGeneratedContent()
    {
        if (m_content.Count > 0 && m_assetPool != null)
        {
            m_assetPool.ReturnToPool(m_content);
            m_content.Clear();
        }
    }

    /// <summary>
    /// Retrieves the currently generated UI objects without modifying the pool or internal state. Returns a copy of the internal list.
    /// </summary>
    public T[] GetGeneratedContent()
    {
        return m_content.ToArray();
    }

    /// <summary>
    /// Removes a specific generated UI object, returning it to the pool and removing it from the internal content list. Returns the updated content.
    /// </summary>
    public T[] RemoveGeneratedObject(T objectToRemove)
    {
        if(m_content.Contains(objectToRemove))
        {
            m_assetPool.ReturnToPool(objectToRemove);
            m_content.Remove(objectToRemove);
        }      

        return m_content.ToArray();
    }

    /// <summary>
    /// Creates a new pooled UI object from the given data. Pulls an instance of type <typeparamref name="T"/> from the pool, initializes it using the
    /// provided data, and adds it to the internal content list.
    /// </summary>
    protected virtual void GeneratePoolableFromData(TData data)
    {
        T newItem = m_assetPool.PullFrom(typeof(T), m_holder) as T;
        newItem.InitializeFromData(data);
        m_content.Add(newItem);
    }
}
