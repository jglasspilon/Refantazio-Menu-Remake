using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private GameStateManager m_gameState;
    private Dictionary<EGameState, HUD> m_huds = new Dictionary<EGameState, HUD>();
    private HUD m_activeHUD;

    private void Awake()
    {
        m_gameState = GameStateManager.Instance;
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
            Debug.LogError($"Failed to change HUD '{gameState}'. '{gameState}' HUD was not present as a child page of the Menu Object.");
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