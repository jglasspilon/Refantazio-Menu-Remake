using Cysharp.Threading.Tasks;
using UnityEngine;

public class ItemSelectionSection : UIObjectSelectionSection<InventoryItem, InventoryItemGenerator, InventoryEntry, InventoryData>
    ,IHandleOnConfirm, IHandleOnBack, IHandlePageLeftLv1, IHandlePageRightLv1
{
    [SerializeField]
    private ContentFramer m_framer;

    [SerializeField]
    private InventoryCategoryCycler m_categoryCycler;

    [SerializeField]
    private Animator m_sectionAnim;

    [SerializeField]
    private AnimatedMover m_bodyMover;

    private IItemSelectable m_parentPage;

    private void Awake()
    {
        m_parentPage = GetComponentInParent<IItemSelectable>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_categoryCycler.InitializeInventoryData(m_dataModel);
        m_categoryCycler.OnCategoryChanged += HandleOnCategoryChanged;
    }

    private void OnDisable()
    {
        m_categoryCycler.OnCategoryChanged -= HandleOnCategoryChanged;
    }

    public override UniTask EnterSection()
    {
        m_sectionAnim.SetBool("CharSection", false);
        m_bodyMover.MoveOut();
        m_selectedIndex = m_selecter.Select(m_selectedIndex);
        return default;
    }

    public override UniTask ExitSection()
    {
        SelectedObject.PauseSelection();
        return default;
    }

    public override void ResetSection()
    {
        m_selecter.UnselectAll();
        m_selectedIndex = 0;
        m_generater.ClearGeneratedContent();
        m_categoryCycler.ResetSelection();
    }

    public void RemoveSpentItem()
    {
        InventoryItem[] updatedList = m_generater.RemoveGeneratedObject(m_selecter.SelectedObject);
        m_selecter.UpdateObjectsAndReturnIndex(updatedList, m_selectedIndex);
    }

    protected override void GenerateUIContent()
    {
        InventoryEntry[] itemsToGenerate = m_dataModel.GetAllItems(m_categoryCycler.Category);
        var generatedItems = m_generater.GenerateContent(itemsToGenerate);
        m_selectedIndex = m_selecter.UpdateObjectsAndReturnIndex(generatedItems, m_selectedIndex);
    }

    protected override void UpdateSelectedObject()
    {
        base.UpdateSelectedObject();
        m_framer.EnsureVisible(m_selecter.SelectedObject.GetComponent<RectTransform>());
    }

    private void HandleOnCategoryChanged(EItemCategories category)
    {
        GenerateUIContent();
        UpdateSelectedObject();
    }

    public void OnConfirm()
    {
        m_parentPage.SelectItem(SelectedObject.InventoryEntry);
    }

    public void OnBack()
    {
        m_parentPage.SelectItem(null);
    }

    public void OnPageLeftLv1()
    {
        m_categoryCycler.CycleCategory(-1, true);
    }

    public void OnPageRightLv1()
    {
        m_categoryCycler.CycleCategory(1, true);
    }    
}
