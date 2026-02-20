using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Serializable]
public class InventoryItemSelecter
{
    [SerializeField][ReadOnly]
    private InventoryItem m_selectedItem;
    
    private InventoryItem[] m_items;

    public InventoryItem SelectedItem => m_selectedItem;

    public int UpdateItemsAndReturnIndex(InventoryItem[] items, int selectedIndex)
    {
        m_items = items;
        return SelectItem(selectedIndex);
    }

    public int SelectItem(int index)
    {
        if (m_items.Length == 0)
            return 0;

        if (index < 0)
            index = m_items.Length - 1;
        else if (index >= m_items.Length)
            index = 0;
       
        if (m_selectedItem != null)
            m_selectedItem.SetAsSelected(false);
        
        m_selectedItem = m_items[index];
        m_selectedItem.SetAsSelected(true);
        return index;
    }
}
