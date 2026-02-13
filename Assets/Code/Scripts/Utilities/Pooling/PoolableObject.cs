using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableObject : MonoBehaviour
{
    [Header("Pooling Settings:")]
    [Range(1, 1000)]
    public int PoolSize = 10;
    [Range(1, 1000)]
    public int MinSpare = 5;

    public abstract void ResetForPool();
}
