using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public interface ISceneLoaderService
{
    public SSceneData CurrentGameplaySceneData { get; }
    public UniTask LoadSceneAsync(EScenes sceneName);
    public UniTask UnLoadSceneAsync(EScenes sceneName);
    public void RegisterSceneLoader(EScenes scene, SceneLoader loader);
    public void UnregisterSceneLoader(EScenes scene);

    public event Action<SSceneData> OnGameplaySceneChanged;
}
