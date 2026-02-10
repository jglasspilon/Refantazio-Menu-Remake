using Cysharp.Threading.Tasks;
using UnityEngine;

public class HUDWidget_Controls : MonoBehaviour, IHUDWidget
{
    [SerializeField]
    private Animator m_anim;

    public EWidgetTypes WidgetType => EWidgetTypes.Controls;

    public async UniTask ShowAsync()
    {
        gameObject.SetActive(true);
        m_anim.SetBool("IsActive", true);
        await Helper.Animation.WaitForCurrentPageAnimationToEnd(m_anim);
    }

    public async UniTask HideAsync()
    {
        m_anim.SetBool("IsActive", false);
        await Helper.Animation.WaitForCurrentPageAnimationToEnd(m_anim);
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        m_anim.SetBool("IsActive", false);
    }
}
