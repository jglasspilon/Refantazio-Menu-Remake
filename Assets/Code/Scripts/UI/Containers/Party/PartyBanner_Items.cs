using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyBanner_Items : PartyBanner
{
    [SerializeField]
    private TextMeshProUGUI m_characterTypeText, m_characterNameText, m_characterHPText, m_characterMPText;

    [SerializeField]
    private Image m_characterBannerImage;

    [SerializeField]
    private Slider m_hpSlider, m_mpSlider;

    [SerializeField]
    private GameObject m_hpSection, m_mpSection, m_guideOverlay;

    public override void InitializeCharacter(Character character)
    {
        base.InitializeCharacter(character);
        m_character.OnTypeChange += DisplayCharacterType;
        m_character.OnHealthChanged += DisplayHP;
        m_character.OnManaChanged += DisplayMp;

        DisplayCharacterType(character.CharacterType);
        DisplayCharacterName(character.Name);
        DisplayBannerSprite(character.Banner);
        DisplayHP(character.CurrentHP, character.CurrentHPProportion);
        DisplayMp(character.CurrentMP, character.CurrentMPProportion);
    }

    public override void ResetForPool()
    {
        m_character.OnTypeChange -= DisplayCharacterType;
        m_character.OnHealthChanged -= DisplayHP;
        m_character.OnManaChanged -= DisplayMp;
        base.ResetForPool();
    }

    private void DisplayCharacterType(ECharacterType characterType)
    {
        string typeString = characterType.ToString();
        string output = $"{typeString.Substring(0,1)}<size=50%>{typeString.Substring(1)}";
        m_characterTypeText.text = output;

        m_hpSection.SetActive(characterType != ECharacterType.Guide);
        m_mpSection.SetActive(characterType != ECharacterType.Guide);
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

    private void DisplayHP(int currentHp, float proportion)
    {
        m_characterHPText.text = currentHp.ToString();
        m_hpSlider.value = proportion;
    }

    private void DisplayMp(int currentMp, float proportion)
    {
        m_characterMPText.text = currentMp.ToString();
        m_mpSlider.value = proportion;
    }
}
