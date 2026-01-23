using System;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public event Action<EGameState> OnGameStateChanged;

    [SerializeField]
    private EGameState m_startingState;

    private EGameState m_currentState;
    private EGameState m_previousState;

    public EGameState CurrentState {  get { return m_currentState; } }

    protected override void Awake()
    {
        base.Awake();
        ChangeState(m_startingState);
    }

    public void ChangeState(EGameState newState)
    {
        m_previousState = m_currentState;
        m_currentState = newState;
        OnGameStateChanged?.Invoke(m_currentState);
    }

    public void ReturnToPreviousState()
    {
        m_currentState = m_previousState;
        OnGameStateChanged?.Invoke(m_currentState);
    }
}

public enum EGameState
{
    Menu,
    Field
}
