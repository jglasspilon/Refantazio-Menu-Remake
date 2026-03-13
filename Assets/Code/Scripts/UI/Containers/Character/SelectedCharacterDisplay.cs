using UnityEngine;

public class SelectedCharacterDisplay : MonoBehaviour
{
    [SerializeField] private PageSection m_parentSection;
    [SerializeField] private CharacterSelecter m_characterSelecter;

    private IBindableToProperty[] m_bindables;

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToProperty>();
    }

    private void OnEnable()
    {
        m_characterSelecter.OnSelectedObjectChanged += SelectCharacter;
        m_parentSection.OnPageLeftLv1 += PreviousCharacter;
        m_parentSection.OnPageRightLv1 += NextCharacter;
    }

    private void OnDisable()
    {
        m_bindables.ForEach(x => x.UnBind());
        m_characterSelecter.OnSelectedObjectChanged -= SelectCharacter;
        m_parentSection.OnPageLeftLv1 -= PreviousCharacter;
        m_parentSection.OnPageRightLv1 -= NextCharacter;
    }

    private void SelectCharacter(CharacterBanner characterProvider)
    {
        if (characterProvider == null || characterProvider.Character == null)
            return;

        Character character = characterProvider.Character;
        m_bindables.ForEach(x => x.UnBind());
        m_bindables.ForEach(x => x.BindToProperty(character));
    }

    private void NextCharacter()
    {
        m_characterSelecter.SelectNext();
    }

    private void PreviousCharacter()
    {
        m_characterSelecter.SelectPrevious();
    }
}
