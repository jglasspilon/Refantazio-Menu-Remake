using UnityEngine;

[RequireComponent(typeof(IGenerater))]
public class GeneraterAnimation: MonoBehaviour
{
    [SerializeField]
    private Animation m_anim;

    private IGenerater m_generater;

    private void Awake()
    {
        m_generater = GetComponent<IGenerater>();
    }

    private void OnEnable()
    {
        m_generater.OnGenerate += PlayAnim;
    }

    private void OnDisable()
    {
        m_generater.OnGenerate -= PlayAnim;
    }

    private void PlayAnim()
    {
        m_anim.Play(PlayMode.StopAll);
    }
}
