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
        GameStateManager.Instance.ChangeState(EGameState.Menu);
    }
}
