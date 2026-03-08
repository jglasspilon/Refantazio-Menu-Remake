using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class ItemSelectionSection : UIListSelectionSection<InventoryItemUI, InventoryItemGenerator, InventoryEntry, InventoryData>
    ,IHandleOnConfirm, IHandleOnBack, IHandlePageLeftLv1, IHandlePageRightLv1
{
    public event Action<InventoryEntry> OnItemSelected;

    [SerializeField]
    private ContentFramer m_framer;

    [Header("Category Management")]
    [SerializeField]
    private EItemCategories m_currentCategory;

    [SerializeField]
    private InventoryCategoryCycler m_categoryCycler;

    [Header("Animations")][SerializeField]
    private Animator m_sectionAnim;

    [SerializeField]
    private AnimatedMover m_bodyMover;   

    protected override void OnEnable()
    {
        base.OnEnable();
        GenerateUIContent();

        if (m_categoryCycler != null)
        {
            m_categoryCycler.InitializeInventoryData(m_dataModel);
            m_categoryCycler.OnCategoryChanged += OnCategoryChanged;
        }

        m_selecter.OnSelectedObjectChanged += FrameSelectedItem;
    }

    private void OnDisable()
    {
        if (m_categoryCycler != null)
        {
            m_categoryCycler.OnCategoryChanged -= OnCategoryChanged;
        }

        m_selecter.OnSelectedObjectChanged -= FrameSelectedItem;
    }

    public override UniTask EnterSection()
    {
        if(m_bodyMover != null)
            m_bodyMover.MoveOut();

        m_sectionAnim.SetBool("CharSection", false);
        m_selecter.SelectCurrent();
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selecter.SelectedObject.PauseSelection();
        return default;
    }

    public override void ResetSection()
    {
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
        m_generater.ClearGeneratedContent();
        m_categoryCycler.ResetSelection();
    }

    protected override void GenerateUIContent()
    {
        if(m_dataModel == null)
        {
            return;
        }

        InventoryEntry[] itemsToGenerate = m_dataModel.GetAllItems(m_currentCategory);
        var generatedItems = m_generater.GenerateContent(itemsToGenerate);
        m_selecter.UpdateObjectsAndReturnIndex(generatedItems);
    }

    private void FrameSelectedItem(InventoryItemUI selectedItem)
    {
        m_framer.EnsureVisible(selectedItem.GetComponent<RectTransform>());
    }

    private void OnCategoryChanged(EItemCategories category)
    {
        m_currentCategory = category;
        GenerateUIContent();
    }

    //TODO: apply in item selecter ---
    public override void HandleOnConfirm()
    {
        base.HandleOnConfirm();
        OnItemSelected?.Invoke(m_selecter.SelectedObject.InventoryEntry);
    }

    public override void HandleOnBack()
    {
        base.HandleOnBack();
        OnItemSelected?.Invoke(null);
    }

    public override void HandleOnPageLeftLv1()
    {
        m_categoryCycler.CycleCategory(-1, true);
        m_selecter.ResetSelecter();
    }

    public override void HandleOnPageRightLv1()
    {
        m_categoryCycler.CycleCategory(1, true);
        m_selecter.ResetSelecter();
    }    
}
