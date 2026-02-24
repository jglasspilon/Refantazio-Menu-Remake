using UnityEngine;

public class PartyBannerGenerator : UIObjectGeneraterFromPool<PartyBanner, Character>
{
    [SerializeField]
    private bool m_activePartyOnly;

    [SerializeField]
    private PartyBanner m_bannerPrefab;

    protected override void GeneratePoolableFromData(Character data)
    {
        if (m_activePartyOnly && data.CharacterType != ECharacterType.Party)
            return;
        PartyBanner newBanner = m_assetPool.PullFrom(m_bannerPrefab.GetType(), m_holder) as PartyBanner;
        newBanner.InitializeFromData(data);
        newBanner.SetAsSelectable(false);
        m_content.Add(newBanner);
    }
}
