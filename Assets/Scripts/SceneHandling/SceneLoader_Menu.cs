using System.Collections;
using UnityEngine;

public class SceneLoader_Menu: SceneLoader
{
    [SerializeField]
    private Menu m_menu;

    public override IEnumerator Load()
    {
        yield break;
    }

    public override IEnumerator Unload()
    {
        yield break;
    }
}
