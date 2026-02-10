using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IHUDWidget
{
    public EWidgetTypes WidgetType { get; }
    public UniTask ShowAsync();
    public UniTask HideAsync();
    public void Hide();
}
