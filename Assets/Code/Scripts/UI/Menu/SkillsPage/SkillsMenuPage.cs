using Cysharp.Threading.Tasks;
using UnityEngine;

public class SkillsMenuPage : MenuPage
{
    [SerializeField]
    private CharacterSelectionSection_Skills m_characterSelectionSection;

    public override UniTask EnterDefaultSection()
    {
        EnterSection(m_characterSelectionSection);
        return default;
    }
}
