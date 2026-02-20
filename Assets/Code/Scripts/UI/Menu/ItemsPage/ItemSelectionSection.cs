using Cysharp.Threading.Tasks;
using UnityEngine;

public class ItemSelectionSection : PageSection, IHandleOnConfirm, IHandleOnBack, IHandleOnCycleUp, IHandleOnCycleDown
{
    [SerializeField]
    private ItemsMenuPage m_parentPage;
    
    [SerializeField]
    private EItemCategories m_selectedCategory;

    [SerializeField]
    private InventoryItemGenerator m_itemGenerater;

    [SerializeField]
    private InventoryItemSelecter m_itemSelecter;

    [SerializeField]
    private ContentFramer m_itemFramer;

    private InventoryData m_inventoryData;
    private AssetPoolManager m_assetPool;
    private int m_selectedItemIndex;

    public InventoryItem SelectedItem => m_itemSelecter.SelectedItem;

    public void OnEnable()
    {
        if (m_assetPool == null)
        {
            if (ObjectResolver.Instance.TryResolve(OnAssetPoolChanged, out AssetPoolManager assetPool))
            {
                OnAssetPoolChanged(assetPool);
            }
        }

        if (m_inventoryData == null)
        {
            if (ObjectResolver.Instance.TryResolve(OnInventoryDataChanged, out InventoryData inventoryData))
            {
                OnInventoryDataChanged(inventoryData);
                return;
            }
        }

        GenerateInventory(m_selectedCategory);
    }

    private void OnInventoryDataChanged(InventoryData inventoryData)
    {
        m_inventoryData = inventoryData;
        m_itemGenerater ??= new InventoryItemGenerator();
        m_itemGenerater.Initialize(inventoryData, m_assetPool);
        GenerateInventory(m_selectedCategory);
    }

    private void OnAssetPoolChanged (AssetPoolManager assetPool)
    {
        m_assetPool = assetPool;
        m_itemGenerater ??= new InventoryItemGenerator();
        m_itemGenerater.Initialize(m_inventoryData, assetPool);
    }

    private void GenerateInventory(EItemCategories category)
    {
        var generatedItems = m_itemGenerater.GenerateInventory(m_selectedCategory);
        m_selectedItemIndex = m_itemSelecter.UpdateItemsAndReturnIndex(generatedItems, m_selectedItemIndex);
    }

    public override UniTask EnterSection()
    {
        return UniTask.WaitForEndOfFrame();
    }

    public override UniTask ExitSection()
    {
        return UniTask.WaitForEndOfFrame();
    }

    public void OnConfirm()
    {
        m_parentPage.SelectItem(SelectedItem.InventoryEntry);
    }

    public void OnBack()
    {
        m_parentPage.SelectItem(null);
    }

    public void OnCycleUp()
    {
        m_selectedItemIndex--;
        UpdateSelectedItem();
    }

    public void OnCycleDown()
    {
        m_selectedItemIndex++;
        UpdateSelectedItem();
    }

    private void UpdateSelectedItem()
    {
        m_selectedItemIndex = m_itemSelecter.SelectItem(m_selectedItemIndex);
        m_itemFramer.EnsureVisible(m_itemSelecter.SelectedItem.GetComponent<RectTransform>());
    }
}
