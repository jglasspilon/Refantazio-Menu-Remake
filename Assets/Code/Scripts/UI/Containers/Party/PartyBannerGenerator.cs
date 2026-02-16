using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PartyBannerGenerator : MonoBehaviour
{
    [SerializeField]
    private bool m_activePartyOnly;

    [SerializeField]
    private PartyBanner m_bannerPrefab;

    [SerializeField]
    private Transform m_bannerHolder;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private List<PartyBanner> m_banners = new List<PartyBanner>();
    private PartyData m_partyData;
    private AssetPoolManager m_assetPool;

    public void OnEnable()
    {
        bool partyDataResolved = m_partyData != null;
        bool assetPoolResolved = m_assetPool != null;

        if (m_partyData == null)
        {
            if (ObjectResolver.Instance.TryResolve(OnPartyDataChanged, out PartyData partyData))
            {
                OnPartyDataChanged(partyData);
                partyDataResolved = true;
            }
        }

        if(m_assetPool == null)
        {
            if (ObjectResolver.Instance.TryResolve(OnAssetPoolChanged, out AssetPoolManager assetPool))
            {
                OnAssetPoolChanged(assetPool);
                assetPoolResolved = true;
            }
        }

        if (!partyDataResolved || !assetPoolResolved)
        {
            return;
        }

        OnPartyCompositionChanged();
    }

    private void OnDisable()
    {
        if (m_partyData == null)
            return;

        if (m_activePartyOnly)
        {
            m_partyData.OnActivePartyChanged -= OnPartyCompositionChanged;
        }
        else
        {
            m_partyData.OnPartyChanged -= OnPartyCompositionChanged;
        }
    }

    private void OnPartyDataChanged(PartyData newReference)
    {
        m_partyData = newReference;

        if (m_activePartyOnly)
        {
            m_partyData.OnActivePartyChanged += OnPartyCompositionChanged;
        }
        else
        {
            m_partyData.OnPartyChanged += OnPartyCompositionChanged;
        }

        OnPartyCompositionChanged();
    }

    private void OnAssetPoolChanged(AssetPoolManager assetPool)
    {
        m_assetPool = assetPool;
    }

    private void OnPartyCompositionChanged()
    {
        if(m_partyData == null)
        {
            Logger.LogError($"Could not update party composition. No Party data registered.", m_logProfile);
            return;
        }

        if(m_assetPool == null)
        {
            Logger.LogError($"Could not update party composition. No asset pool found.", m_logProfile);
            return;
        }

        if(m_banners.Count > 0)
        {
            m_assetPool.ReturnToPool(m_banners);
            m_banners.Clear();
        }

        Character[] charactersToGenerate = m_activePartyOnly ? m_partyData.GetAllActivePartyMembers() : m_partyData.GetAllPartyMembers();
        foreach (Character character in charactersToGenerate)
        {
            GenerateBanner(character);
        }

        if(m_partyData.Guide != null)
        {
            GenerateBanner(m_partyData.Guide);
        }
    }

    private void GenerateBanner(Character character)
    {
        PartyBanner newBanner = m_assetPool.PullFrom(m_bannerPrefab.GetType(), m_bannerHolder) as PartyBanner;
        newBanner.InitializeCharacter(character);
        m_banners.Add(newBanner);
    }
}
