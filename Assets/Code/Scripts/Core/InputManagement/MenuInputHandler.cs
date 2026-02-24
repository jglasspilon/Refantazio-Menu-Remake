using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

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
        m_input.Menu.PageLeftLevel1.performed += OnPageLeftLv1;
        m_input.Menu.PageLeftLevel2.performed += OnPageLeftLv2;
        m_input.Menu.PageRightLevel1.performed += OnPageRightLv1;
        m_input.Menu.PageRightLevel2.performed += OnPageRightLv2;
        m_input.Menu.Exit.performed += OnExit;
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
        m_input.Menu.PageLeftLevel1.performed -= OnPageLeftLv1;
        m_input.Menu.PageLeftLevel2.performed -= OnPageLeftLv2;
        m_input.Menu.PageRightLevel1.performed -= OnPageRightLv1;
        m_input.Menu.PageRightLevel2.performed -= OnPageRightLv2;
        m_input.Menu.Exit.performed -= OnExit;
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

    private void OnPageLeftLv1(InputAction.CallbackContext context)
    {
        m_menu.PageLeftLv1();
        Logger.Log("Page Left Lv 1", gameObject, m_logProfile);
    }

    private void OnPageLeftLv2(InputAction.CallbackContext context)
    {
        m_menu.PageLeftLv2();
        Logger.Log("Page Left Lv 2", gameObject, m_logProfile);
    }

    private void OnPageRightLv1(InputAction.CallbackContext context)
    {
        m_menu.PageRightLv1();
        Logger.Log("Page Right Lv 1", gameObject, m_logProfile);
    }

    private void OnPageRightLv2(InputAction.CallbackContext context)
    {
        m_menu.PageRightLv2();
        Logger.Log("Page Right Lv 2", gameObject, m_logProfile);
    }

    private void OnExit(InputAction.CallbackContext context)
    {
        if (!ObjectResolver.Instance.TryResolve(null, out IGameStateManagementService gameState))
        {
            Logger.LogError("Failed to open menu, no game state management service has been registered", gameObject, m_logProfile);
            return;
        }

        gameState.ReturnToGameplaySate();
        Logger.Log("Exit", gameObject, m_logProfile);
    }    
}
