using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameStateManager : MonoBehaviour, IGameStateManagementService
{
    public event Action<EGameStates> OnGameStateChanged;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private EGameStates m_currentState;
    private EGameStates m_gameplayState = EGameStates.Field;

    public EGameStates CurrentState => m_currentState;

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

    public void ChangeState(EGameStates newState)
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

    public void UpdateGameplayState(EGameStates state, bool changeCurrentState)
    {
        m_gameplayState = state;

        if(changeCurrentState)
        {
            ChangeState(state);
        }
    }   
}
