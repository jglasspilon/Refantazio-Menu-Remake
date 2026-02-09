using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class FieldInputHandler : InputHandler
{
    protected override void BindEvents()
    {
        m_input.Field.OpenMenu.performed += OnOpenMenu;
    }

    protected override void UnBindEvents()
    {
        m_input.Field.OpenMenu.performed -= OnOpenMenu;
    }

    private void OnOpenMenu(InputAction.CallbackContext context)
    {
        if(!ObjectResolver.Instance.TryResolve(null, out IGameStateManagementService gameState))
        {
            Debug.LogError("Failed to open menu, no game state management service has been registered");
            return;
        }

        gameState.ChangeState(EGameState.Menu);
    }
}
