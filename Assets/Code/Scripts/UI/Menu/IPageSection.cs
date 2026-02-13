using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IPageSection
{
    public UniTask Enter();
    public UniTask Exit();
}
