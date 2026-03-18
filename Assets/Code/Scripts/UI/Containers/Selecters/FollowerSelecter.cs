using System;
using UnityEngine;
using UnityEngine.Events;

public class FollowerSelecter : UIObjectSelecter<FollowerBanner>
{
    [SerializeField] private UnityEvent<Follower> OnFollowerChanged;
    public event Action<Follower> OnFollowerSelected;

    protected override void Awake()
    {
        base.Awake();
        m_parentSection.OnConfirm += SelectCharacter;
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
