using System;
using UnityEngine;
using UnityEngine.Events;

public class FollowerSelecter : UIObjectSelecter<FollowerBanner>
{
    [SerializeField] private UnityEvent<Follower> OnFollowerChanged;
    public event Action<Follower> OnFollowerSelected;

    protected void OnEnable()
    {
        m_parentSection.OnConfirm.AddListener(SelectCharacter);
    }

    protected void OnDisable()
    {
        m_parentSection.OnConfirm.RemoveListener(SelectCharacter);
    }

    public void SelectCharacter()
    {
        OnFollowerSelected?.Invoke(SelectedObject == null ? null : SelectedObject.Follower);
    }

    protected override void VirtualInvoke()
    {
        OnFollowerChanged?.Invoke(SelectedObject == null ? null : SelectedObject.Follower);
    }
}
