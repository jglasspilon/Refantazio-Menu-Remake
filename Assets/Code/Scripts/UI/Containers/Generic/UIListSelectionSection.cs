using System;
using UnityEngine;

public abstract class UIListSelectionSection<T, TGenerater, TData, TModel>: PageSection 
    where TGenerater: UIObjectGeneraterFromPool<T, TData>, new()
    where T: PoolableObjectFromData<TData>, ISelectable
{
    [Header("List Creation & Display:")][SerializeField]
    protected TGenerater m_generater;

    [SerializeField]
    protected UIObjectSelecter<T> m_selecter;

    protected TModel m_dataModel;
    protected AssetPoolManager m_assetPool;

    /// <summary>
    /// Provides access to the UI object selector responsible for handling navigation and selection within the generated list.
    /// </summary>
    public UIObjectSelecter<T> Selecter => m_selecter;

    /// <summary>
    /// Called when the section becomes enabled. Attempts to resolve the AssetPoolManager and data model if they have not yet been assigned,
    /// initializing the generator once both dependencies are available.
    /// </summary>
    protected virtual void OnEnable()
    {
        if (m_assetPool == null)
        {
            if (ObjectResolver.Instance.TryResolve(OnAssetPoolChanged, out AssetPoolManager assetPool))
            {
                OnAssetPoolChanged(assetPool);
            }
        }

        if (m_dataModel == null)
        {
            if (ObjectResolver.Instance.TryResolve(OnDataModelChanged, out TModel modelData))
            {
                OnDataModelChanged(modelData);
                return;
            }
        }
    }

    /// <summary>
    /// Callback invoked when the data model is resolved. Stores the model, ensures the generator instance exists, and initializes it using the
    /// current asset pool and owning GameObject.
    /// </summary>
    private void OnDataModelChanged(TModel inventoryData)
    {
        m_dataModel = inventoryData;
        m_generater ??= new TGenerater();
        m_generater.Initialize(m_assetPool, gameObject);
    }

    /// <summary>
    /// Callback invoked when the AssetPoolManager is resolved. Stores the pool reference, ensures the generator instance exists, and initializes it
    /// using the resolved pool and owning GameObject.
    /// </summary>
    private void OnAssetPoolChanged(AssetPoolManager assetPool)
    {
        m_assetPool = assetPool;
        m_generater ??= new TGenerater();
        m_generater.Initialize(assetPool, gameObject);
    }

    /// <summary>
    /// Removes the currently selected UI object from the generated list, returning it to the pool, and updates the selector with the new list.
    /// </summary>
    public void RemoveCurrentSelection()
    {
        T[] updatedList = m_generater.RemoveGeneratedObject(m_selecter.SelectedObject);
        m_selecter.UpdateObjects(updatedList);
    }

    /// <summary>
    /// Must be implemented by derived classes to generate UI content based on the current data model. Typically triggers the generator to create
    /// pooled UI objects and updates the selector accordingly.
    /// </summary>
    protected abstract void GenerateUIContent();
}
