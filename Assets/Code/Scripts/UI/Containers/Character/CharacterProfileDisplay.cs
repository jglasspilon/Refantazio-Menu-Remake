using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CharacterProfileDisplay : MonoBehaviour
{
    [SerializeField] private AnimatedMover m_mover;
    [SerializeField] private Fader m_fader;
    [SerializeField] private CharacterSelecter m_selecter;

    private Image m_image;
    private Character m_shownCharacter;
    private bool m_visible, m_isShowing, m_isHiding;

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        m_selecter.OnSelectedObjectChanged += ChangeCharacter;
    }

    private void OnDisable()
    {
        m_selecter.OnSelectedObjectChanged -= ChangeCharacter;
        m_mover.ResetMover();
        m_shownCharacter = null;
        m_visible = false;
    }

    private void ChangeCharacter(CharacterBanner characterBanner)
    {
        ChangeCharacterAsync(characterBanner);
    }

    private async UniTask ChangeCharacterAsync(CharacterBanner characterBanner)
    {
        if (characterBanner == null || characterBanner.Character == null)
            return;

        Character characterToShow = characterBanner.Character;
        Character currentCharacter = m_shownCharacter;
        m_shownCharacter = characterToShow;

        if (characterToShow == currentCharacter || m_isHiding)
        {
            return;
        }

        if (m_isShowing)
        {
            m_image.sprite = m_shownCharacter.Profile;
            return;
        }

        if (currentCharacter != null)
        {
            await HideCharacter();
        }
        
        await ShowCharacter();
        m_isShowing = false;
    }

    private async UniTask ShowCharacter()
    {
        if (m_visible || !gameObject.activeInHierarchy)
        {
            m_image.sprite = m_shownCharacter.Profile;
            return;
        }

        List<UniTask> tasks = new List<UniTask>();
        m_isShowing = true;
        m_visible = true;
        m_image.sprite = m_shownCharacter.Profile;
        m_isShowing = false;
        tasks.Add(m_mover.MoveInAsync());
        tasks.Add(m_fader.FadeIn());

        await UniTask.WhenAll(tasks);
        m_isShowing = false;
    }

    private async UniTask HideCharacter()
    {
        if (!m_visible || !gameObject.activeInHierarchy)
            return;

        List<UniTask> tasks = new List<UniTask>();
        m_isHiding = true;
        m_visible = false;
        tasks.Add(m_mover.MoveOutAsync());
        tasks.Add(m_fader.FadeOut());

        await UniTask.WhenAll(tasks);
        m_isHiding = false;
    }
}
