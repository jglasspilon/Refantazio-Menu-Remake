using Cysharp.Threading.Tasks;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private SceneData m_sceneData;

    private ISceneLoaderService m_sceneLoaderService;
    private IGameStateManagementService m_gameStateManagementService;

    public SceneData SceneData {  get { return m_sceneData; } }

    protected virtual void Awake()
    {
        m_gameStateManagementService = ObjectResolver.Instance.Resolve<IGameStateManagementService>();
        m_sceneLoaderService = ObjectResolver.Instance.Resolve<ISceneLoaderService>();
        m_sceneLoaderService.RegisterSceneLoader(m_sceneData.Data.SceneName, this);
        Load();
    }

    public virtual void Load()
    {
        m_gameStateManagementService.UpdateGameplayState(m_sceneData.Data.SceneState, m_sceneData.Data.SceneType == ESceneTypes.Gameplay);
    }
    public virtual async UniTask Unload()
    {
        return;
    }
}
