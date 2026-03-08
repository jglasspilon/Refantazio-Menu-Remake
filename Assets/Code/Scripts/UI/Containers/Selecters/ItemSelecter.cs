using System;

public class ItemSelecter : UIObjectSelecter<InventoryItemUI>
{
    public Action<InventoryEntry> OnItemSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += HandleOnConfirm;
        m_parentSection.OnBack += HandleOnBack;
    }

    public void HandleOnConfirm()
    {
        OnItemSelected?.Invoke(SelectedObject.InventoryEntry);
    }

    public void HandleOnBack()
    {
        OnItemSelected?.Invoke(null);
    }
}
