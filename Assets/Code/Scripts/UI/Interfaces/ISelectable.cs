using UnityEngine;

public interface ISelectable
{
    public void SetAsSelected(bool value);
    public void SetAsSelectable(bool selectable);
    public void PauseSelection();
}
