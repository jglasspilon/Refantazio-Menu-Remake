using UnityEngine.InputSystem;
using UnityEngine;

public class MenuInputHandler : InputHandler
{
    [SerializeField]
    private Menu m_menu;

    protected override void BindEvents()
    {
        m_menu.OnPageChange += DisableMenuBinding;
        m_menu.OnPageChangeComplete += EnableMenuBinding;

        EnableMenuBinding();
    }

    protected override void UnBindEvents()
    {
        m_menu.OnPageChange -= DisableMenuBinding;
        m_menu.OnPageChangeComplete -= EnableMenuBinding;

        DisableMenuBinding();
    }

    private void EnableMenuBinding()
    {
        m_input.Menu.CloseMenu.performed += OnCloseMenu;
    }

    private void DisableMenuBinding()
    {
        m_input.Menu.CloseMenu.performed -= OnCloseMenu;
    }

    private void OnCloseMenu(InputAction.CallbackContext context)
    {
        m_menu.Close();
    }
}
