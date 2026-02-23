using Cysharp.Threading.Tasks;
using UnityEngine;

public class ItemSelectionSection : UIObjectSelectionSection<InventoryItem, InventoryItemGenerator, InventoryEntry, InventoryData>
    ,IHandleOnConfirm, IHandleOnBack
{
    [SerializeField]
    private ContentFramer m_framer;
    
    [SerializeField]
    private EItemCategories m_selectedCategory;

    [SerializeField]
    private Animator m_sectionAnim;

    [SerializeField]
    private AnimatedMover m_bodyMover;

    private IItemSelectable m_parentPage;

    private void Awake()
    {
        m_parentPage = GetComponentInParent<IItemSelectable>();
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

    public void OnConfirm()
    {
        m_parentPage.SelectItem(SelectedObject.InventoryEntry);
    }

    public void OnBack()
    {
        m_parentPage.SelectItem(null);
    }

    public void RemoveSpentItem()
    {
        InventoryItem[] updatedList = m_generater.RemoveGeneratedObject(m_selecter.SelectedObject);
        m_selecter.UpdateObjectsAndReturnIndex(updatedList, m_selectedIndex);
    }

    protected override void GenerateUIContent()
    {
        InventoryEntry[] itemsTopGenerate = m_dataModel.GetAllItems(m_selectedCategory);
        var generatedItems = m_generater.GenerateContent(itemsTopGenerate);
        m_selectedIndex = m_selecter.UpdateObjectsAndReturnIndex(generatedItems, m_selectedIndex);
    }

    protected override void UpdateSelectedItem()
    {
        base.UpdateSelectedItem();
        m_framer.EnsureVisible(m_selecter.SelectedObject.GetComponent<RectTransform>());
    }
}
