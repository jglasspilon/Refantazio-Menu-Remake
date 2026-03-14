using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class FollowerTeamData
{
    [SerializeField] private List<Follower> m_followersOrderedList = new List<Follower>();
    [SerializeField] private LoggingProfile m_logProfile;

    private Dictionary<string, Follower> m_followers = new Dictionary<string, Follower>();
    public event Action<Follower> OnFollowerAdded, OnFollowerUnlocked, OnFollowerRankUp;

    public void AddFollower(FollowerData followerData)
    {
        if(m_followers.TryGetValue(followerData.ID, out Follower follower))
        {
            Logger.LogError($"Failed adding follower {followerData.Name}. Follower is already added.", m_logProfile);
            return;
        }

        Follower newFollower = new Follower(followerData);
        m_followers[followerData.ID] = newFollower;
        m_followersOrderedList.Add(newFollower);
    }

    public Follower[] GetAllFollowers()
    {
        return m_followersOrderedList.OrderBy(x => x.SortOrder).ToArray();
    }

    public void UnlockFollower(string id)
    {
        if (m_followers.TryGetValue(id, out Follower follower))
        {
            follower.Unlock();
            return;
        }

        Logger.LogError($"Failed to unlock follower with id {id}. Id not found.", m_logProfile);
    }

    public void UnlockFollower(FollowerData followerData)
    {
        if (m_followers.TryGetValue(followerData.ID, out Follower follower))
        {
            follower.Unlock();
            return;
        }

        Logger.LogError($"Failed to unlock follower {followerData.Name}. Follower not found.", m_logProfile);
    }

    public void UnlockFollower(Follower follower)
    {
        if (m_followers.TryGetValue(follower.ID, out Follower found))
        {
            found.Unlock();
            return;
        }

        Logger.LogError($"Failed to unlock follower {follower.Name}. Follower not found.", m_logProfile);
    }

    public void IncreaseRank(string id)
    {
        if (m_followers.TryGetValue(id, out Follower follower))
        {
            follower.RankUp();
            return;
        }

        Logger.LogError($"Failed to rank up follower with id {id}. Id not found.", m_logProfile);
    }

    public void IncreaseRank(FollowerData followerData)
    {
        if (m_followers.TryGetValue(followerData.ID, out Follower follower))
        {
            follower.RankUp();
            return;
        }

        Logger.LogError($"Failed to rank up follower {followerData.Name}. Follower not found.", m_logProfile);
    }

    public void IncreaseRank(Follower follower)
    {
        if (m_followers.TryGetValue(follower.ID, out Follower found))
        {
            found.RankUp();
            return;
        }

        Logger.LogError($"Failed to rank up follower {follower.Name}. Follower not found.", m_logProfile);
    }
}
