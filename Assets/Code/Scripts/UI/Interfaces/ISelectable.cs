using UnityEngine;

public interface ISelectable
{
    public void SetAsSelected(bool value);
    public void PauseSelection();
    public void Select();
}
