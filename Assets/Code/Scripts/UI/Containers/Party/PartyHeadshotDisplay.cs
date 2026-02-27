using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyHeadshotDisplay : MonoBehaviour
{
    [SerializeField]
    private int m_slotIndex;
    
    [SerializeField]
    private Image m_headshot;

    [SerializeField]
    private TextMeshProUGUI m_positionText;

    [SerializeField]
    private LeftRightMover m_headshotMover, m_footerMover;

    [SerializeField]
    private Color m_frontTextColor, m_backTextColor;

    private Character m_character;
    private PartyData m_partyData;

    private const string FRONT_TEXT = "f<size=60%>ront";
    private const string BACK_TEXT = "b<size=60%>ack";

    private void OnEnable()
    {
        m_partyData ??= ObjectResolver.Instance.Resolve<PartyData>();

        if (m_partyData == null)
            return;

        m_partyData.OnActivePartyChanged += HandlePartyCompositionChanged;
        UpdateCharacterSlot();
    }

    private void ShowCharacter()
    {
        m_headshot.sprite = m_character.Profile;
        m_footerMover.gameObject.SetActive(true);
        m_headshot.gameObject.SetActive(true);
    }

    private void HideCharacter()
    {
        m_footerMover.gameObject.SetActive(false);
        m_headshot.gameObject.SetActive(false);
    }

    private void UpdateCharacterSlot()
    {
        if(m_character != null)
            m_character.OnBattlePositionChange -= HandleOnPositionChange;

        if (m_partyData.TryGetActivePartyMember(m_slotIndex, out m_character))
        {
            m_character.OnBattlePositionChange += HandleOnPositionChange;
            HandleOnPositionChange(m_character.BattlePosition);
            ShowCharacter();
            return;
        }

        HideCharacter();
    }

    private void HandlePartyCompositionChanged()
    {
        UpdateCharacterSlot();
    }

    private void HandleOnPositionChange(EBattlePosition position)
    {
        bool isFront = position == EBattlePosition.Front;
        ECardinalPosition targetPosition = position == EBattlePosition.Front ? ECardinalPosition.Left : ECardinalPosition.Right;

        m_footerMover.SetPosition(targetPosition);
        m_headshotMover.SetPosition(targetPosition);
        m_positionText.text = isFront ? FRONT_TEXT : BACK_TEXT;
        m_positionText.color = isFront ? m_frontTextColor : m_backTextColor;
    }
}
