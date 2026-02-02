using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private LoggingProfile m_logProfile;
    private GameStateManager m_gameState;
    private Dictionary<EGameState, HUD> m_huds = new Dictionary<EGameState, HUD>();
    private HUD m_activeHUD;

    private void Awake()
    {
        m_gameState = GameStateManager.Instance;
        m_huds = GetComponentsInChildren<HUD>(true).ToDictionary(x => x.HUDName, x => x);
        foreach (HUD hud in m_huds.Values)
        {
            hud.Close();
        }
    }

    private void OnEnable()
    {
        m_gameState.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        m_gameState.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(EGameState gameState)
    {
        ChangeHUDContext(gameState);
    }

    private async UniTask ChangeHUDContext(EGameState gameState)
    {
        if (!m_huds.TryGetValue(gameState, out HUD nextHUD))
        {
            string msg = $"Failed to change HUD '{gameState}'. '{gameState}' HUD was not found as a child of this Game Object.";
            Logger.LogError(msg, gameObject, m_logProfile);
            return;
        }

        if (m_activeHUD != null)
        {
            await m_activeHUD.CloseAsync();
        }

        m_activeHUD = nextHUD;
        await m_activeHUD.OpenAsync();
    }
}