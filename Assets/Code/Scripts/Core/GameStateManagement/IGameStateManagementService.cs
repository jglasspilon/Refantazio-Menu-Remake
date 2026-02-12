using System;
using UnityEngine;

public interface IGameStateManagementService: IServiceWithLifecycle
{
    public event Action<EGameStates> OnGameStateChanged;
    public EGameStates CurrentState { get; }

    public void ChangeState(EGameStates newState);
    public void UpdateGameplayState(EGameStates state, bool changeCurrentState);
    public void ReturnToGameplaySate();

}
