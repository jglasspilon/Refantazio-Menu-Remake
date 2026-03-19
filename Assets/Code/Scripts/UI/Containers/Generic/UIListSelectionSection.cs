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

    public UIObjectSelecter<T> Selecter => m_selecter;

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

    private void OnDataModelChanged(TModel inventoryData)
    {
        m_dataModel = inventoryData;
        m_generater ??= new TGenerater();
        m_generater.Initialize(m_assetPool, gameObject);
    }

    private void OnAssetPoolChanged(AssetPoolManager assetPool)
    {
        m_assetPool = assetPool;
        m_generater ??= new TGenerater();
        m_generater.Initialize(assetPool, gameObject);
    }

    public void RemoveCurrentSelection()
    {
        T[] updatedList = m_generater.RemoveGeneratedObject(m_selecter.SelectedObject);
        m_selecter.UpdateObjects(updatedList);
    }

    protected abstract void GenerateUIContent();
}
