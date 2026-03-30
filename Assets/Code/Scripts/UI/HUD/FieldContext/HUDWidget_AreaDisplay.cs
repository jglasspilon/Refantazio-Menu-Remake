using UnityEngine;

public class HUDWidget_AreaDisplay : HUDWidget
{
    private SceneNameBinder[] m_sceneNameBinders;
    private ISceneLoaderService m_sceneLoaderService;

    private void Awake()
    {
        m_sceneNameBinders = GetComponentsInChildren<SceneNameBinder>();
    }

    private void OnEnable()
    {
        m_sceneLoaderService ??= ObjectResolver.Instance.Resolve<ISceneLoaderService>();
        m_sceneLoaderService.OnGameplaySceneChanged += UpdateAreaDisplay;
        UpdateAreaDisplay(m_sceneLoaderService.CurrentGameplaySceneData);
    }

    private void OnDisable()
    {
        if (m_sceneLoaderService == null)
            return;

        m_sceneLoaderService.OnGameplaySceneChanged -= UpdateAreaDisplay;
    }

    private void UpdateAreaDisplay(SSceneData sceneData)
    {
        m_sceneNameBinders.ForEach(x => x.BindAreaDisplay(sceneData));
    }
}
