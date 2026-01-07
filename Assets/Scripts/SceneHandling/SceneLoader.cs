using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Scenes m_sceneName;

    private void Awake()
    {
        SceneManager.Instance.RegisterSceneLoader(m_sceneName, this);
        StartCoroutine(Load());
    }

    public virtual IEnumerator Load()
    {
        yield break;
    }

    public virtual IEnumerator Unload()
    {
        yield break;
    }
}
