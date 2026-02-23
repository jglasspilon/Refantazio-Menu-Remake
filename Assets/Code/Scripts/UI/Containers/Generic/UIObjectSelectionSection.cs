using UnityEngine;

public abstract class UIObjectSelectionSection<T, TGenerater, TData, TModel>: PageSection, IHandleOnCycleDown, IHandleOnCycleUp 
    where TGenerater: UIObjectGeneraterFromPool<T, TData>, new()
    where T: PoolableObjectFromData<TData>, ISelectable
{
    [SerializeField]
    protected TGenerater m_generater;

    [SerializeField]
    protected UIObjectSelecter<T> m_selecter;

    protected int m_selectedIndex;
    protected TModel m_dataModel;
    private AssetPoolManager m_assetPool;

    public T SelectedObject => m_selecter.SelectedObject;

    public void OnEnable()
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
            if (ObjectResolver.Instance.TryResolve(OnDataModelChanged, out TModel inventoryData))
            {
                OnDataModelChanged(inventoryData);
                return;
            }
        }

        GenerateUIContent();
    }

    private void OnDataModelChanged(TModel inventoryData)
    {
        m_dataModel = inventoryData;
        m_generater ??= new TGenerater();
        m_generater.Initialize(m_assetPool, gameObject);
        GenerateUIContent();
    }

    private void OnAssetPoolChanged(AssetPoolManager assetPool)
    {
        m_assetPool = assetPool;
        m_generater ??= new TGenerater();
        m_generater.Initialize(assetPool, gameObject);
    }

    protected abstract void GenerateUIContent();

    public void OnCycleUp()
    {
        m_selectedIndex--;
        UpdateSelectedItem();
    }

    public void OnCycleDown()
    {
        m_selectedIndex++;
        UpdateSelectedItem();
    }

    protected virtual void UpdateSelectedItem()
    {
        m_selectedIndex = m_selecter.Select(m_selectedIndex);
    }
}
