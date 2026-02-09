using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISceneLoaderService
{
    public SceneData CurrentGameplaySceneData { get; }
    public UniTask LoadSceneAsync(EScenes sceneName);
    public UniTask UnLoadSceneAsync(EScenes sceneName);
    public void RegisterSceneLoader(EScenes scene, SceneLoader loader);
    public void UnregisterSceneLoader(EScenes scene);
}
