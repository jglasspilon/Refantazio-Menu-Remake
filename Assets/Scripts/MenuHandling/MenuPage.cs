using System.Collections;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    [SerializeField]
    private Animator m_anim;

    private IUpdateable[] m_updateableGraphics;

    private void Awake()
    {
        m_updateableGraphics = GetComponentsInChildren<IUpdateable>();
    }

    public IEnumerator Open()
    {
        yield break;
    }

    public IEnumerator Close()
    {
        yield break;
    }
}
