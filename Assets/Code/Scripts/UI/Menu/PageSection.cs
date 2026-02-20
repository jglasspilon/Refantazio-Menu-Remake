using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class PageSection: MonoBehaviour
{
    public abstract UniTask EnterSection();
    public abstract UniTask ExitSection();
}
