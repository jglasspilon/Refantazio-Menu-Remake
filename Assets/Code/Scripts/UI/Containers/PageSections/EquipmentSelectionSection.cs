using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentSelectionSection : UIListSelectionSection<InventoryItemUI, InventoryItemGenerator, InventoryEntry, InventoryData>
{
    [SerializeField] private CharacterSelecter m_characterSelecter;
    [SerializeField] private EquipmentSlotSelecter m_slotSelecter;

    private EEquipmentSortType m_sortType;

    public override UniTask EnterSection()
    {
        GenerateUIContent();
        m_onEnter?.Invoke();
        return default;
    }

    public override UniTask ExitSection()
    {
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
        m_onExit?.Invoke();
        return default;
    }

    public override void ResetSection()
    {
        m_selecter.UnselectAll();
        m_selecter.ResetSelecter();
    }

    public void HandleOnSortChanged(EEquipmentSortType sortType)
    {
        m_selecter.ResetSelecter();
        m_sortType = sortType;
        GenerateUIContent();
    }

    protected override void GenerateUIContent()
    {
        if(m_slotSelecter.SelectedObject.SlotContent is not Equipment equipedItem)
        {
            m_generater.ClearGeneratedContent();
            return;
        }

        EEquipmentType typeFilter = equipedItem.Category == EItemCategories.Weapon ? equipedItem.EquipType : EEquipmentType.None;
        InventoryEntry[] itemsToGenerate = m_dataModel.GetAllItems(equipedItem.Category, typeFilter);
        InventoryEntry[] sortedItems; 

        if (m_sortType == EEquipmentSortType.SecondaryStat)
            sortedItems = itemsToGenerate.OrderByDescending(x => (x.Item as Equipment).SecondaryModifier.Amount).ToArray();
        else
            sortedItems = itemsToGenerate.OrderByDescending(x => (x.Item as Equipment).MainModifier.Amount).ToArray();

        InventoryItemUI[] generatedItems = m_generater.GenerateContent(sortedItems);
        m_selecter.UpdateObjects(generatedItems);

        int equipedIndex = -99;

        for (int i = 0; i < generatedItems.Length; i++)
        {
            if (generatedItems[i].InventoryEntry.Item.ID == equipedItem.ID)
            {
                equipedIndex = i;
                m_selecter.Select(i, false);
            }

            if (generatedItems[i] is InventoryItem_Equipable equipable)
            {
                equipable.SetAsEquiped(equipedIndex == i);
                equipable.InitializeWithCharacter(m_characterSelecter.SelectedObject.Character);
            }
        }
    }
}
