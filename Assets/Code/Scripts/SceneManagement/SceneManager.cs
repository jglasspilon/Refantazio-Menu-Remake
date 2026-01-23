using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField]
    private Scenes m_launchScene;

    [SerializeField]
    private SerializedKeyValuePair<Scenes, string>[] m_mappedScenes;

    private Dictionary<Scenes, string> m_mappedScenesDict;
    private Dictionary<string, SceneLoader> m_loadedScenes = new Dictionary<string, SceneLoader>();

    protected override void Awake()
    {
        m_mappedScenesDict = m_mappedScenes.ToDictionary(x => x.Key, x => x.Value);

        base.Awake();
        LoadScene(m_launchScene);
    }

    public void LoadScene(Scenes scene)
    {
        StartCoroutine(LoadSceneAsync(GetSceneName(scene)));
    }

    public void UnloadScene(Scenes scene)
    {
        StartCoroutine(UnLoadSyncAsync(GetSceneName(scene)));
    }

    public void RegisterSceneLoader(Scenes scene, SceneLoader loader)
    {
        string sceneName = scene.ToString();

        if(m_loadedScenes.ContainsKey(sceneName))
        {
            Debug.LogError($"Trying to register an already existing scene loader for '{sceneName}' scene. " +
                $"This is not supported, only a single scene loader should be present for each scene.");

            return;
        }

        m_loadedScenes[sceneName] = loader;
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);

        if(scene.isLoaded)
        {
            yield break;
        }

        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    private IEnumerator UnLoadSyncAsync(string sceneName)
    {
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);

        if (!scene.isLoaded)
        {
            Debug.LogError($"Failed to unload scene, '{sceneName}' is not loaded.");
            yield break;
        }

        if(m_loadedScenes.TryGetValue(sceneName, out SceneLoader loader))
        {
            if (loader == null)
            {
                Debug.LogError($"Failed to to run sceneLoader unload, '{sceneName}' sceneLoader is null.");
            }
            else
            {
                yield return StartCoroutine(loader.Unload());
            }
        }

        yield return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
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
    Menu
}
