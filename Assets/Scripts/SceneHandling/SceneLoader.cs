using System.Collections;
using UnityEngine;

public abstract class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Scenes m_sceneName;

    private void Awake()
    {
        SceneManager.Instance.RegisterSceneLoader(m_sceneName, this);
        StartCoroutine(Load());
    }

    public abstract IEnumerator Load();
    public abstract IEnumerator Unload();
}
