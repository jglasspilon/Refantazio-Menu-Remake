using UnityEngine;

public class ArchetypeBannerGenerator : UIObjectGeneraterFromPool<ArchetypeBanner, Archetype>
{
    [SerializeField] private ArchetypeBanner m_bannerPrefab;

    protected override void GeneratePoolableFromData(Archetype archetype)
    {
        ArchetypeBanner newBanner = m_assetPool.PullFrom(m_bannerPrefab.GetType(), m_holder) as ArchetypeBanner;
        newBanner.InitializeFromData(archetype);
        newBanner.SetAsSelected(false);
        m_content.Add(newBanner);
    }
}
