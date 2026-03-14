using UnityEngine;

public class FollowerBannerGenerator : UIObjectGeneraterFromPool<FollowerBanner, Follower>
{
    [SerializeField]
    private FollowerBanner m_bannerPrefab;

    protected override void GeneratePoolableFromData(Follower follower)
    {
        FollowerBanner newBanner = m_assetPool.PullFrom(m_bannerPrefab.GetType(), m_holder) as FollowerBanner;
        newBanner.InitializeFromData(follower);
        newBanner.SetAsSelectable(false);
        m_content.Add(newBanner);
    }
}
