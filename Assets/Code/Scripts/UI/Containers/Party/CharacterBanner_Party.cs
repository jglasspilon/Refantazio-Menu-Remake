using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharacterBanner_Party : CharacterBanner
{
    [SerializeField]
    private TextMeshProUGUI m_characterNameText, m_characterTypeText, m_levelText;

    [SerializeField]
    private ResourceBar m_characterHp, m_characterMp, m_characterExp;

    [SerializeField]
    private GameObject m_deathIcon, m_characterTypeContent;

    [SerializeField]
    private Image m_archetypeIcon;

    [SerializeField]
    private LeftRightMover m_positionMover;

    [SerializeField]
    private GameObject m_frontContent, m_backContent;
   
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
        m_character.OnBattlePositionChange += DisplayPosition;
        m_character.OnArchetypeChange += DisplayArchetype;
        m_character.OnLevelChange += DisplayLevel;

        m_positionMover.SetEnable(character.CharacterType != ECharacterType.Guide);
        DisplayCharacterType(character.CharacterType);
        DisplayCharacterName(character.Name);
        DisplayDeathIcon(character.IsDead);
        DisplayPosition(character.BattlePosition);
        DisplayLevel(character.Level.Value, 0);
        DisplayArchetype(character.EquipedArchetype);
        SetAsSelected(false);
    }

    public override void ResetForPool()
    {
        m_character.OnTypeChange -= DisplayCharacterType;
        m_character.OnDeath -= DisplayDeathIcon;
        m_character.OnBattlePositionChange -= DisplayPosition;
        m_character.OnArchetypeChange -= DisplayArchetype;
        m_character.OnLevelChange -= DisplayLevel;
        m_characterHp.Unbind();
        m_characterMp.Unbind();
        m_characterExp.Unbind();
        base.ResetForPool();
    }

    #region Display Logic
    private void DisplayCharacterType(ECharacterType characterType)
    {
        bool isGuide = characterType == ECharacterType.Guide;
        string typeString = characterType.ToString();
        string output = $"{typeString.Substring(0,1)}<size=50%>{typeString.Substring(1)}";
        m_characterTypeText.text = output;

        m_characterTypeContent.SetActive(characterType == ECharacterType.Leader || characterType == ECharacterType.Party);
        m_characterHp.gameObject.SetActive(!isGuide);
        m_characterMp.gameObject.SetActive(!isGuide);
        m_characterExp.gameObject.SetActive(!isGuide);
        m_archetypeIcon.gameObject.SetActive(!isGuide);
    }

    private void DisplayCharacterName(string name)
    {
        m_characterNameText.text = name;
    }

    private void DisplayDeathIcon(bool isDead)
    {
        m_deathIcon.SetActive(isDead);
    }

    private void DisplayPosition(EBattlePosition position)
    {
        m_positionMover.SetPosition(position == EBattlePosition.Front ? ECardinalPosition.Left : ECardinalPosition.Right);
        m_frontContent.SetActive(position == EBattlePosition.Front);
        m_backContent.SetActive(position == EBattlePosition.Back); 
    }

    private void DisplayLevel(int level, int levelDelta)
    {
        m_levelText.text = level.ToString("00");
        m_characterExp.Initialize(m_character.Exp);
    }

    private void DisplayArchetype(Archetype archetype)
    {
        m_archetypeIcon.sprite = archetype.Icon;
    }
    #endregion
}
