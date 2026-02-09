using Cysharp.Threading.Tasks;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Scenes m_sceneName;

    [SerializeField]
    private EGameState m_sceneState;

    private ISceneLoaderService m_sceneLoaderService;
    private IGameStateManagementService m_gameStateManagementService;

    protected virtual void Awake()
    {
        m_gameStateManagementService = ObjectResolver.Instance.Resolve<IGameStateManagementService>();
        m_sceneLoaderService = ObjectResolver.Instance.Resolve<ISceneLoaderService>();
        m_sceneLoaderService.RegisterSceneLoader(m_sceneName, this);
        Load();
    }

    public virtual void Load()
    {
        m_gameStateManagementService.UpdateGameplayState(m_sceneState);
    }
    public virtual async UniTask Unload()
    {
        return;
    }
}
