using System;
using UnityEngine;

public interface IGameStateManagementService: IServiceWithLifecycle
{
    public event Action<EGameState> OnGameStateChanged;
    public EGameState CurrentState { get; }

    public void ChangeState(EGameState newState);
    public void UpdateGameplayState(EGameState state);
    public void ReturnToGameplaySate();

}
