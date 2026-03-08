using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CharacterArmDisplay : MonoBehaviour
{
    [SerializeField]
    private CharacterSelecter m_casterSelecter;

    [SerializeField]
    private Image m_armImage;

    [SerializeField]
    private AnimatedMover m_mover;

    [SerializeField]
    private Fader m_fader;

    [SerializeField]
    private float m_startDelaySeconds = 2;

    private Character m_armOwner;
    private CancellationTokenSource m_cts;

    private void OnEnable()
    {
        m_armImage.enabled = false;

        m_cts?.Cancel();
        m_cts = new CancellationTokenSource();
        DelayArmSelectionOnStart(m_startDelaySeconds, m_cts.Token);
    }

    private void OnDisable()
    {
        m_cts?.Cancel();
        m_casterSelecter.OnSelectedObjectChanged -= Display;
        m_armImage.enabled = false;
    }

    private async void Display(CharacterBanner characterBanner)
    {
        if (characterBanner == null)
            return;

        Character character = characterBanner.Character;

        if (character == m_armOwner)
            return;

        Hide();
        
        m_armImage.enabled = character.Arm != null;
        m_armImage.sprite = character.Arm;
        m_armOwner = character;

        if (character.Arm != null)
            await Show();
    }

    private async UniTask Show()
    {
        UniTask moveTask = m_mover.MoveInAsync();
        UniTask fadeTask = m_fader.FadeIn();
        UniTask[] waitFor = new UniTask[] { moveTask, fadeTask };
        await UniTask.WhenAll(waitFor);
    }

    private void Hide()
    {
        m_mover.ResetMover();
        m_fader.ResetFader(false);
    }

    private async UniTask DelayArmSelectionOnStart(float delay, CancellationToken token)
    {
        float timer = delay;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        m_casterSelecter. OnSelectedObjectChanged += Display;
        Display(m_casterSelecter.SelectedObject);
    }
}
