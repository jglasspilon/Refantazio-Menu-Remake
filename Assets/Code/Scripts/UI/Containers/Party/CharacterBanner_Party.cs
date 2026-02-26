using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBanner_Party : CharacterBanner
{
    [SerializeField]
    private TextMeshProUGUI m_characterNameText, m_characterTypeText, m_levelText;

    [SerializeField]
    private ResourceBar m_characterHp, m_characterMp, m_characterExp;

    [SerializeField]
    private GameObject m_deathIcon, m_characterTypeContent, m_bg;

    [SerializeField]
    private GameObject[] m_contentOnSelectedOnly;

    [SerializeField]
    private FrontBackMover m_positionMover;
   
    [SerializeField]
    private UIEffect[] m_healEffect;

    [SerializeField]
    private UIEffect[] m_damageEffect;

    [SerializeField]
    private UIEffect[] m_manaRestoreEffect;

    public override void InitializeFromData(Character character)
    {
        base.InitializeFromData(character);
        m_characterHp.Initialize(character.HP);
        m_characterMp.Initialize(character.MP);
        m_characterExp.Initialize(character.Exp);
        m_character.OnTypeChange += DisplayCharacterType;
        m_character.OnDeath += DisplayDeathIcon;
        m_character.OnBattlePositionChange += m_positionMover.SetPosition;

        m_positionMover.SetEnable(character.CharacterType != ECharacterType.Guide);
        m_positionMover.SetPosition(character.BattlePosition);
        DisplayCharacterType(character.CharacterType);
        DisplayCharacterName(character.Name);
        DisplayDeathIcon(character.IsDead);
        DisplayLevel(character.Stats.Level.Value);
        SetAsSelected(false);
    }

    public override void ResetForPool()
    {
        m_character.OnTypeChange -= DisplayCharacterType;
        m_character.OnDeath -= DisplayDeathIcon;
        m_character.OnBattlePositionChange -= m_positionMover.SetPosition;
        m_characterHp.Unbind();
        m_characterMp.Unbind();
        m_characterExp.Unbind();
        base.ResetForPool();
    }

    #region Selectable Logic
    public override void SetAsSelected(bool value)
    {
        m_positionMover.SetAsSelected(value);
        m_contentOnSelectedOnly.ForEach(x => x.SetActive(value));
        m_bg.SetActive(!value);
    }

    public override void SetAsSelectable(bool selectable)
    {
        
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
        m_characterExp.gameObject.SetActive(characterType != ECharacterType.Guide);
    }

    private void DisplayCharacterName(string name)
    {
        m_characterNameText.text = name;
    }

    private void DisplayDeathIcon(bool isDead)
    {
        m_deathIcon.SetActive(isDead);
    }

    private void DisplayLevel(int level)
    {
        m_levelText.text = level.ToString("00");
    }
    #endregion
}
