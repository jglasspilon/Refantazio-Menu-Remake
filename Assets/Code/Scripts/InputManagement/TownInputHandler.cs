using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class TownInputHandler : InputHandler
{
    [SerializeField]
    private LoggingProfile m_logProfile;

    protected override void BindEvents()
    {
        m_input.Town.OpenMenu.performed += OnOpenMenu;
    }

    protected override void UnBindEvents()
    {
        m_input.Town.OpenMenu.performed -= OnOpenMenu;
    }

    private void OnOpenMenu(InputAction.CallbackContext context)
    {
        if(!ObjectResolver.Instance.TryResolve(null, out IGameStateManagementService gameState))
        {
            Logger.LogError("Failed to open menu, no game state management service has been registered", gameObject, m_logProfile);
            return;
        }

        gameState.ChangeState(EGameState.Menu);
        Logger.Log("Open Menu", gameObject, m_logProfile);
    }
}
