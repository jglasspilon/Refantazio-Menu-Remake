using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;

public class ItemSelectionSection : UIListSelectionSection<InventoryItemUI, InventoryItemGenerator, InventoryEntry, InventoryData>
    ,IHandleOnConfirm, IHandleOnBack, IHandlePageLeftLv1, IHandlePageRightLv1
{
    [SerializeField]
    private InventoryCategoryCycler m_categoryCycler;

    [Header("Animations")][SerializeField]
    private Animator m_sectionAnim;

    [Space][SerializeField]
    private UnityEvent OnEnter;

    protected override void OnEnable()
    {
        base.OnEnable();
        GenerateUIContent();

        if (m_categoryCycler != null)
        {
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
        OnEnter?.Invoke();
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

        InventoryEntry[] itemsToGenerate = m_dataModel.GetAllItems(m_categoryCycler.Category);
        var generatedItems = m_generater.GenerateContent(itemsToGenerate);
        m_selecter.UpdateObjects(generatedItems);
        m_selecter.SelectCurrent();
    }

    private void OnCategoryChanged(EItemCategories category)
    {
        m_selecter.ResetSelecter();
        GenerateUIContent();
    }
}
