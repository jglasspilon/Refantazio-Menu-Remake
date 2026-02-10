using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameStateManager : MonoBehaviour, IGameStateManagementService
{
    public event Action<EGameState> OnGameStateChanged;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private EGameState m_currentState;
    private EGameState m_gameplayState = EGameState.Field;

    public EGameState CurrentState { get { return m_currentState; } }

    private void Awake()
    {
        ObjectResolver.Instance.Register<IGameStateManagementService>(this);        
    }

    public void Initialize()
    {
        
    }

    public void Shutdown()
    {
        OnGameStateChanged = null;
    }

    public void ChangeState(EGameState newState)
    {
        m_currentState = newState;
        OnGameStateChanged?.Invoke(m_currentState);
        Logger.Log($"Changed game state to '{newState}'.", gameObject, m_logProfile);
    }

    public void ReturnToGameplaySate()
    {
        m_currentState = m_gameplayState;
        OnGameStateChanged?.Invoke(m_currentState);
        Logger.Log($"Returned to gameplay state '{m_gameplayState}'.", gameObject, m_logProfile);
    }

    public void UpdateGameplayState(EGameState state, bool changeCurrentState)
    {
        m_gameplayState = state;

        if(changeCurrentState)
        {
            ChangeState(state);
        }
    }   
}

public enum EGameState
{
    Menu,
    Field, 
    Town,
    Combat,
    Dialoque,
    Cinematic,
    Launch
}
