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

        if (m_categoryCycler != null)
        {
            m_categoryCycler.InitializeInventoryData(m_dataModel);
            m_categoryCycler.OnCategoryChanged += OnCategoryChanged;
        }
    }

    private void OnDisable()
    {
        if (m_categoryCycler != null)
        {
            m_categoryCycler.OnCategoryChanged -= OnCategoryChanged;
        }
    }

    public override UniTask EnterSection()
    {
        if(m_bodyMover != null)
            m_bodyMover.MoveOut();

        m_sectionAnim.SetBool("CharSection", false);
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

    protected override void GenerateUIContent()
    {
        InventoryEntry[] itemsToGenerate = m_dataModel.GetAllItems(m_currentCategory);
        var generatedItems = m_generater.GenerateContent(itemsToGenerate);
        m_selectedIndex = m_selecter.UpdateObjectsAndReturnIndex(generatedItems, m_selectedIndex);
    }

    protected override void UpdateSelectedObject()
    {
        base.UpdateSelectedObject();
        m_framer.EnsureVisible(m_selecter.SelectedObject.GetComponent<RectTransform>());
    }

    private void OnCategoryChanged(EItemCategories category)
    {
        m_currentCategory = category;
        GenerateUIContent();
        UpdateSelectedObject();
    }

    public void OnConfirm()
    {
        OnItemSelected?.Invoke(SelectedObject.InventoryEntry);
    }

    public void OnBack()
    {
        OnItemSelected?.Invoke(null);
    }

    public void OnPageLeftLv1()
    {
        m_categoryCycler.CycleCategory(-1, true);
        m_selectedIndex = m_selecter.Select(0, false);
    }

    public void OnPageRightLv1()
    {
        m_categoryCycler.CycleCategory(1, true);
        m_selecter.Select(0, false);
        m_selectedIndex = m_selecter.Select(0, false);
    }    
}
