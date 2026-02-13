using System;
using UnityEngine;

public abstract class InputHandler : MonoBehaviour
{
    protected GameInput m_input;
    protected IInputManagementService m_inputManager;

    private void OnEnable()
    {
        if (ObjectResolver.Instance.TryResolve(OnInputManagerChanged, out IInputManagementService inputManager))
        {
            OnInputManagerChanged(inputManager);
        }
    }

    private void OnDisable()
    {
        UnBindEvents();
    }

    private void OnInputManagerChanged(IInputManagementService newReference)
    {
        m_inputManager = newReference;
        m_input = m_inputManager.InputActions;
        BindEvents();
    }

    protected abstract void BindEvents();
    protected abstract void UnBindEvents();
}
