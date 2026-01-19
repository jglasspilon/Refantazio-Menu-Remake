using UnityEngine.InputSystem;

public class PlayerInputHandler : InputHandler
{
    protected override void BindEvents()
    {
        m_input.Player.OpenMenu.performed += OnOpenMenu;
        m_inputManager.SetToPlayerInputs();
    }

    protected override void UnBindEvents()
    {
        m_input.Player.OpenMenu.performed -= OnOpenMenu;
    }

    private void OnOpenMenu(InputAction.CallbackContext context)
    {
        SceneManager.Instance.LoadScene(Scenes.Menu);
    }
}
