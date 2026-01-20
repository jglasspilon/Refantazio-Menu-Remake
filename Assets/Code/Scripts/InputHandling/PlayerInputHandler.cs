using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInputHandler : InputHandler
{
    [SerializeField]
    private Menu m_menu;

    protected override void BindEvents()
    {
        m_input.Player.OpenMenu.performed += OnOpenMenu;
    }

    protected override void UnBindEvents()
    {
        m_input.Player.OpenMenu.performed -= OnOpenMenu;
    }

    private void OnOpenMenu(InputAction.CallbackContext context)
    {
        m_menu.Launch();
    }
}
