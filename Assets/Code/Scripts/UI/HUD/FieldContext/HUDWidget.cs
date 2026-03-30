using Cysharp.Threading.Tasks;
using UnityEngine;

public class HUDWidget : MonoBehaviour, IHUDWidget
{
    [SerializeField]
    private Animator m_anim;

    [SerializeField]
    private EWidgetTypes m_widgetType;

    public EWidgetTypes WidgetType => m_widgetType;

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
