using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyBanner_Items : PartyBanner
{
    [Header("Character Banner:")]
    [SerializeField]
    private Image m_characterBannerImage;

    [SerializeField]
    private TextMeshProUGUI m_characterNameText, m_characterTypeText, m_characterMPText;

    [SerializeField]
    private ResourceBar m_characterHp, m_characterMp;

    [SerializeField]
    private GameObject m_guideOverlay, m_deathIcon, m_characterTypeContent, m_bg;

    [SerializeField]
    private GameObject[] m_contentOnSelectedOnly;
   
    [SerializeField]
    private PartyBannerEffect[] m_healEffect;

    [SerializeField]
    private PartyBannerEffect[] m_damageEffect;

    [SerializeField]
    private PartyBannerEffect[] m_manaRestoreEffect;

    public override void InitializeFromData(Character character)
    {
        base.InitializeFromData(character);
        m_characterHp.Initialize(character.HP);
        m_characterMp.Initialize(character.MP);
        m_character.OnTypeChange += DisplayCharacterType;
        m_character.OnDeath += DisplayDeathIcon;

        DisplayCharacterType(character.CharacterType);
        DisplayCharacterName(character.Name);
        DisplayBannerSprite(character.Banner);
        DisplayDeathIcon(character.IsDead);
        SetAsSelected(false);
    }

    public override void ResetForPool()
    {
        m_character.OnTypeChange -= DisplayCharacterType;
        m_character.OnDeath -= DisplayDeathIcon;
        m_characterHp.Unbind();
        m_characterMp.Unbind();
        base.ResetForPool();
    }

    #region Selectable Logic
    public override void SetAsSelected(bool value)
    {
        m_contentOnSelectedOnly.ForEach(x => x.SetActive(value));
    }

    public override void SetAsSelectable(bool selectable)
    {
        m_bg.SetActive(!selectable);
    }

    public override void PauseSelection()
    {
        
    }
    #endregion

    #region Display Logic
    private void DisplayCharacterType(ECharacterType characterType)
    {
        string typeString = characterType.ToString();
        string output = $"{typeString.Substring(0,1)}<size=50%>{typeString.Substring(1)}";
        m_characterTypeText.text = output;

        m_characterTypeContent.SetActive(characterType == ECharacterType.Leader || characterType == ECharacterType.Party);
        m_characterHp.gameObject.SetActive(characterType != ECharacterType.Guide);
        m_characterMp.gameObject.SetActive(characterType != ECharacterType.Guide);
        m_guideOverlay.SetActive(characterType == ECharacterType.Guide);
    }

    private void DisplayCharacterName(string name)
    {
        m_characterNameText.text = name;
    }

    private void DisplayBannerSprite(Sprite sprite)
    {
        m_characterBannerImage.sprite = sprite;
    }

    private void DisplayDeathIcon(bool isDead)
    {
        m_deathIcon.SetActive(isDead);
    }
    #endregion
}
