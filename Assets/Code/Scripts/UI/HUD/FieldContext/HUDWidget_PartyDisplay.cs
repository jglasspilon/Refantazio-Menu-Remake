using UnityEngine;

public class HUDWidget_PartyDisplay : HUDWidget
{
    private CharacterBanner[] m_banners;
    private PartyData m_partyData;

    private void Awake()
    {
        m_banners = GetComponentsInChildren<CharacterBanner>();
    }

    private void OnEnable()
    {
        if(m_partyData == null)
        {
            if (ObjectResolver.Instance.TryResolve(UpdatePartyData, out PartyData partyData))
            {
                UpdatePartyData(partyData);
                return;
            }

            return;
        }

        UpdatePartyData(m_partyData);
    }

    private void OnDisable()
    {
        if (m_partyData == null)
            return;
        
        m_partyData.OnActivePartyChanged -= DisplayParty;
    }

    private void UpdatePartyData(PartyData partyData)
    {
        if (m_partyData != null)
            m_partyData.OnActivePartyChanged -= DisplayParty;

        m_partyData = partyData;
        m_partyData.OnActivePartyChanged += DisplayParty;
        DisplayParty();
    }

    private void DisplayParty()
    {
        for(int i = 0; i < m_banners.Length; i++)
        {
            CharacterBanner banner = m_banners[i];

            if (m_partyData.TryGetActivePartyMember(i, out Character partyMember))
            {
                banner.InitializeFromData(partyMember);
                banner.gameObject.SetActive(true);
            }
            else
            {
                banner.gameObject.SetActive(false);
            }
        }
    }
}
