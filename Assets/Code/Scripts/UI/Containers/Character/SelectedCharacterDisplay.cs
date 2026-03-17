using UnityEngine;

public class SelectedCharacterDisplay : MonoBehaviour
{ 
    [SerializeField] private CharacterSelecter m_characterSelecter;

    private IBindableToProperty[] m_bindables;

    private void Awake()
    {
        m_bindables = GetComponentsInChildren<IBindableToProperty>();
    }

    private void OnEnable()
    {
        m_characterSelecter.OnSelectedObjectChanged += SelectCharacter;
    }

    private void OnDisable()
    {
        m_bindables.ForEach(x => x.UnBind());
        m_characterSelecter.OnSelectedObjectChanged -= SelectCharacter;
    }

    private void SelectCharacter(CharacterBanner characterProvider)
    {
        if (characterProvider == null || characterProvider.Character == null)
            return;

        Character character = characterProvider.Character;
        m_bindables.ForEach(x => x.BindToProperty(character));
    }
}
