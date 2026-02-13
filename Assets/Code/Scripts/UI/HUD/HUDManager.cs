using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private LoggingProfile m_logProfile;
    private IGameStateManagementService m_gameState;
    private Dictionary<EGameStates, HUDContext> m_huds = new Dictionary<EGameStates, HUDContext>();
    private HUDContext m_activeHUD;

    private void Awake()
    {
        m_huds = GetComponentsInChildren<HUDContext>(true).ToDictionary(x => x.HUDName, x => x);
        foreach (HUDContext hud in m_huds.Values)
        {
            hud.Close();
        }

        if(ObjectResolver.Instance.TryResolve(OnGameStateManagerChanged, out IGameStateManagementService gameState))
        {
            OnGameStateManagerChanged(gameState);
        }
    }

    private void OnDestroy()
    {
        if (m_gameState != null)
        {
            m_gameState.OnGameStateChanged -= OnGameStateChanged;
        }
    }

    private void OnGameStateManagerChanged(IGameStateManagementService newReference)
    {
        m_gameState = newReference;
        m_gameState.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(EGameStates gameState)
    {
        ChangeHUDContext(gameState);
    }

    private async UniTask ChangeHUDContext(EGameStates gameState)
    {
        if (!m_huds.TryGetValue(gameState, out HUDContext nextHUD))
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