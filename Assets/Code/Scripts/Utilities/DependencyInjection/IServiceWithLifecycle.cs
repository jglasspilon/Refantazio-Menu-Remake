using UnityEngine;

public interface IServiceWithLifecycle
{
    public void Initialize();
    public void Shutdown();
}
