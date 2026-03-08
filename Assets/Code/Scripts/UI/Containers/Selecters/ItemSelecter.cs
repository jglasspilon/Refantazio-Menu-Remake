using System;

public class ItemSelecter : UIObjectSelecter<InventoryItemUI>
{
    public Action<InventoryEntry> OnItemSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += HandleOnConfirm;
    }

    public void HandleOnConfirm()
    {
        OnItemSelected?.Invoke(SelectedObject.InventoryEntry);
    }
}
