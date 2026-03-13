using UnityEngine;

public class ABBoolAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_anim;
    [SerializeField] private string m_boolName;
    [SerializeField] private bool m_aBoolValue;

    public void PlayA()
    {
        if (m_anim == null)
            return;

        m_anim.SetBool(m_boolName, m_aBoolValue);
    }

    public void PlayB()
    {
        if (m_anim == null)
            return;

        m_anim.SetBool(m_boolName, !m_aBoolValue);
    }
}
