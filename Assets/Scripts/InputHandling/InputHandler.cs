using UnityEngine;

public abstract class InputHandler : MonoBehaviour
{
    protected GameInput m_input;
    protected InputManager m_inputManager;

    private void OnEnable()
    {
        if (m_inputManager == null)
        {
            m_inputManager = InputManager.Instance;
        }

        if (m_input == null)
        {
            m_input = m_inputManager.InputActions;
        }

        BindEvents();
    }

    private void OnDisable()
    {
        UnBindEvents();
    }

    protected abstract void BindEvents();
    protected abstract void UnBindEvents();
}
