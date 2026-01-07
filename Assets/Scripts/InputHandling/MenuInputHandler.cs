using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class MenuInputHandler : MonoBehaviour
{
    private GameInput m_input;
    private InputManager m_inputManager;
    private SceneManager m_sceneManager;

    private void OnEnable()
    {
        if(m_sceneManager == null)
        {
            m_sceneManager = SceneManager.Instance;
        }

        if (m_inputManager == null)
        {
            m_inputManager = InputManager.Instance;
        }

        if (m_input == null)
        {
            m_input = m_inputManager.InputActions;
        }

        m_input.Menu.CloseMenu.performed += OnCloseMenu;
        m_inputManager.SetToMenuInputs();
    }

    private void OnDisable()
    {
        m_input.Menu.CloseMenu.performed -= OnCloseMenu;
        m_inputManager.SetToPlayerInputs();
    }

    private void OnCloseMenu(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        m_sceneManager.UnloadScene(Scenes.Menu);
    }
}
