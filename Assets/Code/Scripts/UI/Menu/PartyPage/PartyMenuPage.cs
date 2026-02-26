using Cysharp.Threading.Tasks;
using UnityEngine;

public class PartyMenuPage : MenuPage
{
    [SerializeField]
    private CharacterSelectionSection_Party m_characterSelectionSection;

    public override UniTask EnterDefaultSection()
    {
        EnterSection(m_characterSelectionSection);
        return default;
    }
}
