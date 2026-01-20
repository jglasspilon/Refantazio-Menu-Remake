using System.Collections;
using UnityEngine;

public abstract class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Scenes m_sceneName;

    private void Awake()
    {
        SceneManager.Instance.RegisterSceneLoader(m_sceneName, this);
        Load();
    }

    public abstract void Load();
    public abstract IEnumerator Unload();
}
