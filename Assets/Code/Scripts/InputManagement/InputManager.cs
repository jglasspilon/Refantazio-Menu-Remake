using System;
using UnityEngine;

public class InputManager : MonoBehaviour, IInputManagementService
{
    private GameInput m_input;
    private IGameStateManagementService m_gameState;

    public GameInput InputActions {get { return m_input; } }

    private void Awake()
    {
        m_input = new GameInput();
        ObjectResolver.Instance.Register<IInputManagementService>(this);       

        if(ObjectResolver.Instance.TryResolve(OnGameStateManagerChanged, out m_gameState))
        {
            m_gameState.OnGameStateChanged += SetInputMapFromGameState;
            SetInputMapFromGameState(m_gameState.CurrentState);
        }      
    }

    private void OnDestroy()
    {
        if (m_gameState != null)
        {
            m_gameState.OnGameStateChanged -= SetInputMapFromGameState;
        }
    }

    private void OnGameStateManagerChanged()
    {
        m_gameState = ObjectResolver.Instance.Resolve<IGameStateManagementService>();
        m_gameState.OnGameStateChanged += SetInputMapFromGameState;
        SetInputMapFromGameState(m_gameState.CurrentState);
    }

    private void SetInputMapFromGameState(EGameState state)
    {
        m_input.Field.Disable();
        m_input.Menu.Disable();

        switch(state)
        {          
            case EGameState.Field:
                m_input.Field.Enable();
                break;
            case EGameState.Menu:
                m_input.Menu.Enable();
                break;
        }
    }
}

public enum EInputMap
{
    Field,
    Menu
}
