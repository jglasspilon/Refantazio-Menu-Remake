using UnityEngine.InputSystem;

public class MenuInputHandler : InputHandler
{
    protected override void BindEvents()
    {
        m_input.Menu.CloseMenu.performed += OnCloseMenu;
        m_inputManager.SetToMenuInputs();
    }

    protected override void UnBindEvents()
    {
        m_input.Menu.CloseMenu.performed -= OnCloseMenu;
        m_inputManager.SetToPlayerInputs();
    }

    private void OnCloseMenu(InputAction.CallbackContext context)
    {
        SceneManager.Instance.UnloadScene(Scenes.Menu);
    }
}
