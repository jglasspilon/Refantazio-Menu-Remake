using System;
using UnityEngine;

public interface ISelectable
{
    public event Action<bool> OnSetAsSelected, OnSetAsSelectable;
    public void SetAsSelected(bool value);
    public void SetAsSelectable(bool selectable);
    public void PauseSelection();
}
