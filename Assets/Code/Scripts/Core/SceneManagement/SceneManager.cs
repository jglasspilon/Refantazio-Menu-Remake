using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour, ISceneLoaderService
{
    [SerializeField]
    private EScenes m_launchScene;

    [SerializeField]
    private SerializedKeyValuePair<EScenes, string>[] m_mappedScenes;

    [SerializeField]
    private LoggingProfile m_logProfile;

    private Dictionary<EScenes, string> m_mappedScenesDict;
    private Dictionary<string, SceneLoader> m_loadedScenes = new Dictionary<string, SceneLoader>();
    private SceneData m_currentGameplaySceneData;

    public SceneData CurrentGameplaySceneData => m_currentGameplaySceneData;

    protected void Awake()
    {
        ObjectResolver.Instance.Register<ISceneLoaderService>(this);
        m_mappedScenesDict = m_mappedScenes.ToDictionary(x => x.Key, x => x.Value);
        LoadSceneAsync(m_launchScene);
    }

    private void OnDestroy()
    {
        ObjectResolver.Instance.Unregister<ISceneLoaderService>();
    }

    public void RegisterSceneLoader(EScenes scene, SceneLoader loader)
    {
        string sceneName = scene.ToString();

        if(m_loadedScenes.ContainsKey(sceneName))
        {
            Logger.LogError($"Trying to register an already existing scene loader for '{sceneName}' scene. This is not supported, only a single scene loader should be present for each scene.", gameObject, m_logProfile);
            return;
        }

        m_loadedScenes[sceneName] = loader;

        if (loader.SceneData.Data.SceneType == ESceneTypes.Gameplay)
            m_currentGameplaySceneData = loader.SceneData;
    }

    public void UnregisterSceneLoader(EScenes scene)
    {
        string sceneName = scene.ToString();

        if (!m_loadedScenes.ContainsKey(sceneName))
        {
            Logger.LogError($"Trying to unregister '{sceneName}' which is not registered.", gameObject, m_logProfile);
            return;
        }

        m_loadedScenes.Remove(sceneName);
    }

    public async UniTask LoadSceneAsync(EScenes sceneName)
    {
        string sceneString = GetSceneName(sceneName);
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneString);

        if(scene.isLoaded)
        {
            return;
        }

        Logger.Log($"Stared loading '{sceneName}' scene.", gameObject, m_logProfile);
        await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneString, LoadSceneMode.Additive);
        Logger.Log($"Loaded '{sceneName}' scene succesfully.", gameObject, m_logProfile);
    }

    public async UniTask UnLoadSceneAsync(EScenes sceneName)
    {
        string sceneString = GetSceneName(sceneName);
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneString);

        if (!scene.isLoaded)
        {
            Logger.LogError($"Failed to unload scene, '{sceneName}' is not loaded.", gameObject, m_logProfile);
            return;
        }

        if(m_loadedScenes.TryGetValue(sceneString, out SceneLoader loader))
        {
            if (loader == null)
            {
                Logger.LogError($"Failed to to run sceneLoader unload, '{sceneName}' sceneLoader is null.", gameObject, m_logProfile);
            }
            else
            {
                await loader.Unload();
                UnregisterSceneLoader(sceneName);
            }
        }

        Logger.Log($"Stared unloading '{sceneName}' scene.", gameObject, m_logProfile);
        await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneString);
        Logger.Log($"Unloaded '{sceneName}' scene succesfully.", gameObject, m_logProfile);
    }

    private string GetSceneName(EScenes scene)
    {
        if(!m_mappedScenesDict.ContainsKey(scene))
        {
            Logger.LogError($"Failed to find mapping for {scene}. Will try to parse to string but result may not be as expected.", gameObject, m_logProfile);
            return scene.ToString();
        }

        return m_mappedScenesDict[scene];
    }
}

public enum EScenes
{
    Launch,
    Demo, 
}

public enum ESceneTypes
{
    Gameplay,
    UI,
    Launch
}
