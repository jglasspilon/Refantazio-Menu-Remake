using System;
using UnityEngine;

public class FollowerSelecter : UIObjectSelecter<FollowerBanner>
{
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
}
