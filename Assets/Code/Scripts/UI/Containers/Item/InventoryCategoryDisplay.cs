using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCategoryDisplay : MonoBehaviour
{
    [SerializeField]
    private EItemCategories m_category;

    [SerializeField]
    private Image m_categoryIcon;

    [SerializeField]
    private Sprite m_activeSprite, m_inactiveSprite;

    [SerializeField]
    private float m_activeScaleFactor, m_inactiveScaleFactor;

    [SerializeField]
    private GameObject m_isSeenIcon, m_unavailableOverlay;

    [SerializeField]
    private InventoryCategoryCycler m_categroyCycler;

    private InventoryData m_data;

    private void OnEnable()
    {
        m_data ??= ObjectResolver.Instance.Resolve<InventoryData>();
        m_categroyCycler.OnCategoryChanged += DisplayAsSelected;
        DisplayAsSelected(m_categroyCycler.Category);

        if (m_data == null)
            return;

        m_data.OnLastMarkUnseen += DisplayUnseenIcon;
        InventoryEntry[] entriesForCategory = m_data.GetAllItems(m_category).Where(x => x.Count > 0).ToArray();
        m_unavailableOverlay.SetActive(entriesForCategory.Count() == 0);
        m_isSeenIcon.SetActive(entriesForCategory.Where(x => x.IsNew).Count() > 0);
    }

    private void OnDisable()
    {
        m_categroyCycler.OnCategoryChanged -= DisplayAsSelected;
        m_data.OnLastMarkUnseen -= DisplayUnseenIcon;
    }

    private void DisplayAsSelected(EItemCategories selectedCategory)
    {
        bool isSelected = m_category == selectedCategory;
        float scaleFactor = isSelected ? m_activeScaleFactor : m_inactiveScaleFactor;
        m_categoryIcon.sprite = isSelected ? m_activeSprite : m_inactiveSprite;
        m_categoryIcon.transform.localScale = Vector3.one * scaleFactor;
    }

    private void DisplayUnseenIcon(EItemCategories isSeenCategory)
    {
        if (m_category == isSeenCategory)
        {
            m_isSeenIcon.SetActive(false);
        }
    }
}
