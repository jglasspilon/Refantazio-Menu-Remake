using System;
using System.Linq;
using UnityEngine;

public class InventoryCategoryCycler: MonoBehaviour
{
    public event Action<EItemCategories> OnCategoryChanged;

    [SerializeField][ReadOnly]
    private EItemCategories m_category = EItemCategories.Usable;

    private InventoryData m_data;
    private int m_selectedCategoryIndex;
    private readonly int CATEGORIES_COUNT = Enum.GetValues(typeof(EItemCategories)).Length;
    public EItemCategories Category => m_category;

    public void InitializeInventoryData(InventoryData data)
    {
        m_data = data;
    }

    public void ResetSelection()
    {
        m_selectedCategoryIndex = 0;
        m_category = EItemCategories.Usable;
    }

    public void CycleCategory(int amount, bool cycle)
    {
        if(m_data.GetAllItems(EItemCategories.All).Where(x => x.Count > 0).Count() == 0)
        {
            return;
        }

        m_selectedCategoryIndex = Helper.Arrays.GetSafeIndex(m_selectedCategoryIndex + amount, CATEGORIES_COUNT, cycle);
        m_category = (EItemCategories)m_selectedCategoryIndex;

        if(m_category == EItemCategories.All)
        {
            CycleCategory(amount, cycle);
            return;
        }

        if (m_data.GetAllItems(m_category).Where(x => x.Count > 0).Count() > 0)
        {
            OnCategoryChanged?.Invoke(m_category);
            return;
        }

        CycleCategory(amount, cycle);
    }
}
