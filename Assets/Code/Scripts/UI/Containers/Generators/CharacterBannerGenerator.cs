using UnityEngine;

public class CharacterBannerGenerator : UIObjectGeneraterFromPool<CharacterBanner, Character>
{
    [SerializeField]
    private bool m_activePartyOnly;

    [SerializeField]
    private CharacterBanner m_bannerPrefab;

    protected override void GeneratePoolableFromData(Character character)
    {
        if (m_activePartyOnly && !character.IsInActiveParty)
            return;

        CharacterBanner newBanner = m_assetPool.PullFrom(m_bannerPrefab.GetType(), m_holder) as CharacterBanner;
        newBanner.InitializeFromData(character);
        newBanner.SetAsSelectable(false);
        m_content.Add(newBanner);
    }
}
