using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour, ISceneLoaderService
{
    [SerializeField]
    private Scenes m_launchScene;

    [SerializeField]
    private SerializedKeyValuePair<Scenes, string>[] m_mappedScenes;

    private Dictionary<Scenes, string> m_mappedScenesDict;
    private Dictionary<string, SceneLoader> m_loadedScenes = new Dictionary<string, SceneLoader>();

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

    public void RegisterSceneLoader(Scenes scene, SceneLoader loader)
    {
        string sceneName = scene.ToString();

        if(m_loadedScenes.ContainsKey(sceneName))
        {
            Debug.LogError($"Trying to register an already existing scene loader for '{sceneName}' scene. This is not supported, only a single scene loader should be present for each scene.");
            return;
        }

        m_loadedScenes[sceneName] = loader;
    }

    public void UnregisterSceneLoader(Scenes scene)
    {
        string sceneName = scene.ToString();

        if (!m_loadedScenes.ContainsKey(sceneName))
        {
            Debug.LogError($"Trying to unregister '{sceneName}' which is not registered.");
            return;
        }

        m_loadedScenes.Remove(sceneName);
    }

    public async UniTask LoadSceneAsync(Scenes sceneName)
    {
        string sceneString = GetSceneName(sceneName);
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneString);

        if(scene.isLoaded)
        {
            return;
        }

        await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneString, LoadSceneMode.Additive);
    }

    public async UniTask UnLoadSceneAsync(Scenes sceneName)
    {
        string sceneString = GetSceneName(sceneName);
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneString);

        if (!scene.isLoaded)
        {
            Debug.LogError($"Failed to unload scene, '{sceneName}' is not loaded.");
            return;
        }

        if(m_loadedScenes.TryGetValue(sceneString, out SceneLoader loader))
        {
            if (loader == null)
            {
                Debug.LogError($"Failed to to run sceneLoader unload, '{sceneName}' sceneLoader is null.");
            }
            else
            {
                await loader.Unload();
                UnregisterSceneLoader(sceneName);
            }
        }

        await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneString);
    }

    private string GetSceneName(Scenes scene)
    {
        if(!m_mappedScenesDict.ContainsKey(scene))
        {
            Debug.LogError($"Failed to find mapping for {scene}. Will try to parse to string but result may not be as expected.");
            return scene.ToString();
        }

        return m_mappedScenesDict[scene];
    }
}

public enum Scenes
{
    Launch,
    Demo, 
}
