using System.Collections.Generic;
using UnityEngine;

public class FollowerManager : MonoBehaviour
{
    [SerializeField] private FollowerTeamData m_followers;
    [SerializeField] private FollowerData[] m_followersToAdd;
    [SerializeField] private FollowerData[] m_followersToUnlock;

    private void Awake()
    {
        ObjectResolver.Instance.Register(m_followers);
    }

    private void Start()
    {
        m_followersToAdd.ForEach(f => m_followers.AddFollower(f));
        m_followersToUnlock.ForEach(f => m_followers.UnlockFollower(f));
    }
}
