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
        m_input.Menu.Confirm.performed += OnConfirm;
        m_input.Menu.CycleUp.performed += OnCycleUp;
        m_input.Menu.CycleDown.performed += OnCycleDown;
        m_input.Menu.Back.performed += OnBack;
    }

    private void DisableMenuBinding()
    {
        m_input.Menu.Confirm.performed -= OnConfirm;
        m_input.Menu.CycleUp.performed -= OnCycleUp;
        m_input.Menu.CycleDown.performed -= OnCycleDown;
        m_input.Menu.Back.performed -= OnBack;
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        m_menu.Confirm();
    }

    private void OnCycleUp(InputAction.CallbackContext context)
    {
        m_menu.CycleUp();
    }

    private void OnCycleDown(InputAction.CallbackContext context)
    {
        m_menu.CycleDown();
    }

    private void OnBack(InputAction.CallbackContext context)
    {
        m_menu.Back();
    }
}
