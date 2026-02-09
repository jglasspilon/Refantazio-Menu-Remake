using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour, IGameStateManagementService
{
    public event Action<EGameState> OnGameStateChanged;

    [SerializeField]
    private EGameState m_startingState;

    private EGameState m_currentState;
    private EGameState m_gameplayState;

    public EGameState CurrentState { get { return m_currentState; } }

    private void Awake()
    {
        ObjectResolver.Instance.Register<IGameStateManagementService>(this);        
    }

    public void Initialize()
    {
        ChangeState(m_startingState);
    }

    public void Shutdown()
    {
        OnGameStateChanged = null;
    }

    public void ChangeState(EGameState newState)
    {
        m_currentState = newState;
        OnGameStateChanged?.Invoke(m_currentState);
    }

    public void ReturnToGameplaySate()
    {
        m_currentState = m_gameplayState;
        OnGameStateChanged?.Invoke(m_currentState);
    }

    public void UpdateGameplayState(EGameState state)
    {
        m_gameplayState = state;
    }

    
}

public enum EGameState
{
    Menu,
    Field, 
    Town,
    Combat,
    Dialoque,
    Cinematic
}
