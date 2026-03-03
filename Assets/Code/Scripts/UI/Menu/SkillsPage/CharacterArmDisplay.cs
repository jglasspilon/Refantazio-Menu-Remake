using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CharacterArmDisplay : MonoBehaviour
{
    [SerializeField]
    private SkillsMenuPage m_parentPage;

    [SerializeField]
    private Image m_armImage;

    [SerializeField]
    private AnimatedMover m_mover;

    [SerializeField]
    private Fader m_fader;

    [SerializeField]
    private float m_startDelaySeconds = 2;

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
        m_parentPage.OnCasterChange -= Display;
        m_armImage.enabled = false;
    }

    private async void Display(Character character)
    {
        Hide();

        if (character == null)
            return;      

        m_armImage.enabled = character.Arm != null;
        m_armImage.sprite = character.Arm;

        if (character.Arm != null)
            await Show();
    }

    private async UniTask Show()
    {
        UniTask moveTask = m_mover.MoveIn();
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

        m_parentPage.OnCasterChange += Display;
        Display(m_parentPage.Caster);
    }
}
