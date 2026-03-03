using UnityEngine;

public class CharacterBannerGenerator : UIObjectGeneraterFromPool<CharacterBanner, Character>
{
    [SerializeField]
    private bool m_activePartyOnly;

    [SerializeField]
    private CharacterBanner m_bannerPrefab;

    protected override void GeneratePoolableFromData(Character data)
    {
        if (m_activePartyOnly && data.CharacterType != ECharacterType.Party)
            return;
        CharacterBanner newBanner = m_assetPool.PullFrom(m_bannerPrefab.GetType(), m_holder) as CharacterBanner;
        newBanner.InitializeFromData(data);
        newBanner.SetAsSelectable(false);
        m_content.Add(newBanner);
    }
}
