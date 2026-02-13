using System;
using UnityEngine;

public class InputManager : MonoBehaviour, IInputManagementService
{
    private GameInput m_input;
    private IGameStateManagementService m_gameState;

    public GameInput InputActions => m_input;

    private void Awake()
    {
        m_input = new GameInput();
        ObjectResolver.Instance.Register<IInputManagementService>(this);       

        if(ObjectResolver.Instance.TryResolve(OnGameStateManagerChanged, out IGameStateManagementService gameState))
        {
            OnGameStateManagerChanged(gameState);
        }      
    }

    private void OnDestroy()
    {
        if (m_gameState != null)
        {
            m_gameState.OnGameStateChanged -= SetInputMapFromGameState;
        }
    }

    private void OnGameStateManagerChanged(IGameStateManagementService newReference)
    {
        m_gameState = newReference;
        m_gameState.OnGameStateChanged += SetInputMapFromGameState;
        SetInputMapFromGameState(m_gameState.CurrentState);
    }

    private void SetInputMapFromGameState(EGameStates state)
    {
        m_input.Field.Disable();
        m_input.Menu.Disable();
        m_input.Town.Disable();

        switch(state)
        {
            case EGameStates.Town:
                m_input.Town.Enable();
                break;
            case EGameStates.Field:
                m_input.Field.Enable();
                break;
            case EGameStates.Menu:
                m_input.Menu.Enable();
                break;
        }
    }
}
