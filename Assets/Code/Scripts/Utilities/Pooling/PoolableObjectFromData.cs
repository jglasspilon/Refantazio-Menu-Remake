using UnityEngine;

public abstract class PoolableObjectFromData<T>: PoolableObject
{
    public abstract void InitializeFromData(T data);
}
