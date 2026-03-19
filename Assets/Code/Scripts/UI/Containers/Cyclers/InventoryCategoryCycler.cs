using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PageSection))]
[DisallowMultipleComponent]
public class InventoryCategoryCycler: MonoBehaviour
{
    private PageSection m_parentSection;
    private EItemCategories m_category = EItemCategories.Usable;
    private InventoryData m_data;
    private int m_selectedCategoryIndex;
    private readonly int CATEGORIES_COUNT = Enum.GetValues(typeof(EItemCategories)).Length;

    public event Action<EItemCategories> OnCategoryChanged;

    public EItemCategories Category => m_category;

    private void Awake()
    {
        m_parentSection = GetComponent<PageSection>();        
    }

    private void OnEnable()
    {
        m_data = ObjectResolver.Instance.Resolve<InventoryData>();
        m_parentSection.OnPageLeftLv1.AddListener(CycleCategoryLeft);
        m_parentSection.OnPageRightLv1.AddListener(CycleCategoryRight);
    }

    private void OnDisable()
    {
        m_parentSection.OnPageLeftLv1.RemoveListener(CycleCategoryLeft);
        m_parentSection.OnPageRightLv1.RemoveListener(CycleCategoryRight);
    }

    public void ResetSelection()
    {
        m_selectedCategoryIndex = 0;
        m_category = EItemCategories.Usable;
    }

    private void CycleCategoryLeft()
    {
        CycleCategory(-1, true);
    }

    private void CycleCategoryRight()
    {
        CycleCategory(1, true);
    }

    private void CycleCategory(int amount, bool cycle)
    {
        if(m_data.GetAllItems(EItemCategories.All).Where(x => x.Count > 0).Count() == 0)
        {
            return;
        }

        m_selectedCategoryIndex = Helper.Arrays.GetSafeIndex(m_selectedCategoryIndex + amount, CATEGORIES_COUNT, cycle);
        m_category = (EItemCategories)m_selectedCategoryIndex;

        if(m_category == EItemCategories.All) //Skip all category
        {
            CycleCategory(amount, cycle);
            return;
        }

        if (m_data.GetAllItems(m_category).Where(x => x.Count > 0).Count() == 0) //Skip empty category
        {
            CycleCategory(amount, cycle);            
            return;
        }

        OnCategoryChanged?.Invoke(m_category);
    }
}
