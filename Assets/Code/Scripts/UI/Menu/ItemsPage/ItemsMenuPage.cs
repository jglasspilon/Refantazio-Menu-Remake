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

    public override UniTask EnterDefaultSection()
    {
        EnterSection(m_itemSelectionSection);
        return default;
    }

    public void SelectItem(InventoryEntry item)
    {
        if (item.Item is not UsableItem usable || usable.BattleOnly)
            return;

        m_selectedItem = item;
        EnterSection(m_characterSelectionSection);
    }

    public override void ResetPage()
    {
        
    }
}
