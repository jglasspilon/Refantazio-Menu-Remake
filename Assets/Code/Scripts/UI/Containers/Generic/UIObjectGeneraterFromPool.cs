using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIObjectGeneraterFromPool<T, TData> : MonoBehaviour, IDisposable where T : PoolableObjectFromData<TData>
{
    public event Action OnGenerated;

    [SerializeField]
    protected Transform m_holder;

    [SerializeField]
    protected Animation m_onGeneratedAnim;

    [SerializeField]
    protected LoggingProfile m_logProfile;

    protected AssetPoolManager m_assetPool;
    protected List<T> m_content = new List<T>();  
    protected GameObject m_owner;

    public UIObjectGeneraterFromPool()
    {

    }

    public void Dispose()
    {
        ClearGeneratedContent();
    }

    public void Initialize(AssetPoolManager assetPool, GameObject owner)
    {
        m_assetPool = assetPool;
        m_owner = owner;
    }

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

        if (m_onGeneratedAnim != null)
            m_onGeneratedAnim.Play(PlayMode.StopAll);

        OnGenerated?.Invoke();
        return m_content.ToArray();
    }

    public void ClearGeneratedContent()
    {
        if (m_content.Count > 0 && m_assetPool != null)
        {
            m_assetPool.ReturnToPool(m_content);
            m_content.Clear();
        }
    }

    public T[] RemoveGeneratedObject(T objectToRemove)
    {
        if(m_content.Contains(objectToRemove))
        {
            m_assetPool.ReturnToPool(objectToRemove);
            m_content.Remove(objectToRemove);
        }      

        return m_content.ToArray();
    }

    protected virtual void GeneratePoolableFromData(TData data)
    {
        T newItem = m_assetPool.PullFrom(typeof(T), m_holder) as T;
        newItem.InitializeFromData(data);
        m_content.Add(newItem);
    }
}
