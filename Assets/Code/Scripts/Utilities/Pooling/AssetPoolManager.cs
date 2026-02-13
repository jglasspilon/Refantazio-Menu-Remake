using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssetPoolManager : MonoBehaviour
{
    [SerializeField]
    private string[] m_prefabsResourcePaths = new string[] { "PooledPrefabs" };

    private List<PoolableObject> m_poolPrefabs = new List<PoolableObject>();
    private Dictionary<Type, List<PoolableObject>> m_pools;
    private Dictionary<Type, PoolableObject> m_mappedPoolPrefabs;

    private void Awake()
    {
        ObjectResolver.Instance.Register(this);

        foreach (string resourcesPath in m_prefabsResourcePaths)
            m_poolPrefabs.AddRange(Resources.LoadAll<PoolableObject>(resourcesPath).ToArray());

        m_pools = new Dictionary<Type, List<PoolableObject>>();
        m_mappedPoolPrefabs = m_poolPrefabs.ToDictionary(x => x.GetType(), x => x);

        FillPools();
    }

    private void FillPools()
    {
        foreach (var prefab in m_poolPrefabs)
        {
            if (!m_pools.ContainsKey(prefab.GetType()))
            {
                m_pools.Add(prefab.GetType(), new List<PoolableObject>());
                for (int i = 0; i < prefab.PoolSize; i++)
                {
                    PoolableObject newInstance = Instantiate(prefab, this.transform);
                    newInstance.gameObject.SetActive(false);
                    m_pools[prefab.GetType()].Add(newInstance);
                }
            }
        }
    }

    private void CheckPool(Type type, int min)
    {
        if (!m_pools.ContainsKey(type))
        {
            Debug.LogError($"Pool not found for type {type}");
            return;
        }
  

        while ((m_pools[type].Count - min) < m_mappedPoolPrefabs[type].MinSpare)
        {
            PoolableObject newInstance = Instantiate(m_mappedPoolPrefabs[type], this.transform);
            newInstance.gameObject.SetActive(false);
            m_pools[type].Add(Instantiate(newInstance));
        }
    }

    public T PullFrom<T>(Transform parent = null) where T : PoolableObject
    {
        if (!m_pools.ContainsKey(typeof(T)))
        {
            Debug.LogError($"Pool not found for type {typeof(T)}");
            return null;
        }
        
        CheckPool(typeof(T), 1);
        
        T tmp = m_pools[typeof(T)][0] as T;
        m_pools[typeof(T)].RemoveAt(0);
        PrepareForPull(tmp, parent);

        return tmp;
    }

    public PoolableObject PullFrom(Type poolableObjectType, Transform parent = null)
    {
        if (!m_pools.ContainsKey(poolableObjectType))
        {
            Debug.LogError($"Pool not found for type {poolableObjectType}");
            return null;
        }

        CheckPool(poolableObjectType, 1);

        PoolableObject tmp = m_pools[poolableObjectType][0];
        m_pools[poolableObjectType].RemoveAt(0);
        PrepareForPull(tmp, parent);

        return tmp;
    }

    public T[] PullFromMulti<T>(int count, Transform parent = null) where T : PoolableObject
    {
        if (!m_pools.ContainsKey(typeof(T)))
        {
            Debug.LogError($"Pool not found for type {typeof(T)}");
            return null;
        }

        CheckPool(typeof(T), count);

        T[] tmp = Array.ConvertAll(m_pools[typeof(T)].GetRange(0, count).ToArray(), x => (T) x);
        m_pools[typeof(T)].RemoveRange(0, count);

        foreach (T entry in tmp)
            PrepareForPull(entry, parent);

        return tmp;
    }

    public PoolableObject[] PullFromMulti(Type poolableObjectType, int count, Transform parent = null)
    {
        if (!m_pools.ContainsKey(poolableObjectType))
        {
            Debug.LogError($"Pool not found for type {poolableObjectType}");
            return null;
        }

        CheckPool(poolableObjectType, count);

        PoolableObject[] tmp = m_pools[poolableObjectType].GetRange(0, count).ToArray();
        m_pools[poolableObjectType].RemoveRange(0, count);

        foreach (PoolableObject entry in tmp)
            PrepareForPull(entry, parent);

        return tmp;
    }

    public void ReturnToPool(PoolableObject returned)
    {
        if (!m_pools.ContainsKey(returned.GetType()))
        {
            Debug.LogError($"Pool not found for type {returned.GetType()}");
            return;
        }

        CleanupFromReturn(returned);
    }

    public void ReturnToPool(IEnumerable<PoolableObject> returned)
    {
        if(returned == null || !returned.Any())
        {
            Debug.LogError($"Failed to return to pool. Empty or no array provided.");
            return;
        }

        Type assetType = returned.First().GetType();
        if (!m_pools.ContainsKey(assetType))
        {
            Debug.LogError($"Pool not found for type {assetType}");
            return;
        }

        foreach(PoolableObject o in returned)
            CleanupFromReturn(o);
    }

    private void PrepareForPull(PoolableObject poolable, Transform parent)
    {
        if (parent != null)
        {
            poolable.transform.SetParent(parent);
            poolable.transform.localPosition = Vector3.zero;
        }
        poolable.gameObject.SetActive(true);
    }

    private void CleanupFromReturn(PoolableObject returned)
    {
        returned.ResetForPool();
        returned.transform.SetParent(this.transform);
        returned.transform.localPosition = Vector3.zero;
        returned.gameObject.SetActive(false);
        if (!m_pools[returned.GetType()].Contains(returned))
        {
            m_pools[returned.GetType()].Add(returned);
        }
    }
}
