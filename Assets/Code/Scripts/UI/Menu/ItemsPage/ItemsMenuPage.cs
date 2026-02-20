using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class ItemsMenuPage : MenuPage
{
    public event Action<int> OnItemIndexChange, OnCharacterIndexChange;
    public event Action<int> OnCategoryIndexChange;

    [SerializeField]
    private ItemSelectionSection m_itemSelectionSection;

    [SerializeField]
    private PageSection m_characterSelectionSection;

    private InventoryEntry m_selectedItem;
    private Character m_selectedCharacter;

    protected override PageSection CurrentPageSection => m_breadcrumb.Count > 0 ? m_breadcrumb.Peek() : m_itemSelectionSection;

    public override UniTask EnterDefaultSection()
    {
        m_itemSelectionSection.EnterSection();
        return default;
    }

    public void SelectItem(InventoryEntry item)
    {
        if (item.Item is not UsableItem usable || usable.BattleOnly)
            return;

        m_selectedItem = item;
    }

    public override void ResetPage()
    {
        
    }
}
