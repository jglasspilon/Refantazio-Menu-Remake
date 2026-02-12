using UnityEngine.InputSystem;
using UnityEngine;

public class MenuInputHandler : InputHandler
{
    [SerializeField]
    private MenuManager m_menu;

    [SerializeField]
    private LoggingProfile m_logProfile;

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
        if (m_inputManager == null)
        {
            Logger.LogError("Failed to enable menu binding, input manager unresolved.", gameObject, m_logProfile);
            return;
        }

        m_input.Menu.Confirm.performed += OnConfirm;
        m_input.Menu.CycleUp.performed += OnCycleUp;
        m_input.Menu.CycleDown.performed += OnCycleDown;
        m_input.Menu.Back.performed += OnBack;
        Logger.Log("Enabled Menu inputs", gameObject, m_logProfile);
    }

    private void DisableMenuBinding()
    {
        if (m_inputManager == null)
        {
            Logger.LogError("Failed to disabled menu binding, input manager unresolved.", gameObject, m_logProfile);
            return;
        }

        m_input.Menu.Confirm.performed -= OnConfirm;
        m_input.Menu.CycleUp.performed -= OnCycleUp;
        m_input.Menu.CycleDown.performed -= OnCycleDown;
        m_input.Menu.Back.performed -= OnBack;
        Logger.Log("Disabled Menu inputs", gameObject, m_logProfile);
    }

    private void OnConfirm(InputAction.CallbackContext context)
    {
        m_menu.Confirm();
        Logger.Log("Confirm", gameObject, m_logProfile);
    }

    private void OnCycleUp(InputAction.CallbackContext context)
    {
        m_menu.CycleUp();
        Logger.Log("Cycle Up", gameObject, m_logProfile);
    }

    private void OnCycleDown(InputAction.CallbackContext context)
    {
        m_menu.CycleDown();
        Logger.Log("Cycle Down", gameObject, m_logProfile);
    }

    private void OnBack(InputAction.CallbackContext context)
    {
        m_menu.Back();
        Logger.Log("Back", gameObject, m_logProfile);
    }
}
