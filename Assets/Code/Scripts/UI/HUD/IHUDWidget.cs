using Cysharp.Threading.Tasks;
using UnityEngine;

public interface HUDWidget
{
    public EWidgetTypes WidgetType { get; }
    public UniTask ShowAsync();
    public UniTask HideAsync();
    public void Hide();
}
