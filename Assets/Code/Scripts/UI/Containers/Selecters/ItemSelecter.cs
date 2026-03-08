using System;

public class ItemSelecter : UIObjectSelecter<InventoryItemUI>
{
    public Action<InventoryEntry> OnItemSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += SelectItem;
    }

    public void SelectItem()
    {
        OnItemSelected?.Invoke(SelectedObject.InventoryEntry);
    }
}
