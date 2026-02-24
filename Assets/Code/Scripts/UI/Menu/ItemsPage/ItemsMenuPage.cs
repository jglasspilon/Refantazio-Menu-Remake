using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class ItemsMenuPage : MenuPage, IItemSelectable, ICharacterSelectable
{
    public event Action<int> OnItemIndexChange, OnCharacterIndexChange;
    public event Action<int> OnCategoryIndexChange;

    [SerializeField]
    private ItemSelectionSection m_itemSelectionSection;

    [SerializeField]
    private CharacterSelectionSection m_characterSelectionSection;

    private InventoryEntry m_selectedItem;
    private ItemExecutor m_itemExecutor = new ItemExecutor();

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

        if(usable.TargetingType == ETargetingTypes.AllAllies || usable.TargetingType == ETargetingTypes.All)
        {
            m_characterSelectionSection.SelectAll();
        }

        EnterSection(m_characterSelectionSection);
    }

    public void SelectCharacter(Character character)
    {
        Logger.Log($"Using {m_selectedItem.Item.Name} on {character.Name}", m_logProfile);        
        UseItem(m_selectedItem.Item as UsableItem, character);

        if (m_selectedItem.Count == 0)
        {
            m_itemSelectionSection.RemoveSpentItem();
            TryExitCurrentSection();
            return;
        }

        //TODO: add if no more selectable characters from item condition, go back
    }


    private void UseItem(UsableItem item, Character character)
    {
        m_selectedItem.ApplyAmount(-1);

        //TODO: use item executer
    }
}
