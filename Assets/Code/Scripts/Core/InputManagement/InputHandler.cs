using System;
using UnityEngine;

public abstract class InputHandler : MonoBehaviour
{
    protected GameInput m_input;
    protected IInputManagementService m_inputManager;

    private void OnEnable()
    {
        if (ObjectResolver.Instance.TryResolve(OnInputManagerChanged, out m_inputManager))
        {
            m_input = m_inputManager.InputActions;
            BindEvents();
            return;
        }
    }

    private void OnDisable()
    {
        UnBindEvents();
    }

    private void OnInputManagerChanged()
    {
        m_inputManager = ObjectResolver.Instance.Resolve<IInputManagementService>();
        m_input = m_inputManager.InputActions;
        BindEvents();
    }

    protected abstract void BindEvents();
    protected abstract void UnBindEvents();
}
