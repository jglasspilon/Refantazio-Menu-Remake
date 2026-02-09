using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISceneLoaderService
{
    public UniTask LoadSceneAsync(Scenes sceneName);
    public UniTask UnLoadSceneAsync(Scenes sceneName);
    public void RegisterSceneLoader(Scenes scene, SceneLoader loader);
    public void UnregisterSceneLoader(Scenes scene);
}
