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
        if(m_sceneLoaderService == null)
        {
            if (ObjectResolver.Instance.TryResolve(UpdateSceneLoaderService, out ISceneLoaderService loaderService))
            {
                UpdateSceneLoaderService(loaderService);
                return;
            }

            return;
        }

        UpdateSceneLoaderService(m_sceneLoaderService);
    }

    private void OnDisable()
    {
        if (m_sceneLoaderService == null)
            return;

        m_sceneLoaderService.OnGameplaySceneChanged -= UpdateAreaDisplay;
    }

    private void UpdateSceneLoaderService(ISceneLoaderService service)
    {
        if (m_sceneLoaderService != null)
            m_sceneLoaderService.OnGameplaySceneChanged -= UpdateAreaDisplay;

        m_sceneLoaderService = service;
        m_sceneLoaderService.OnGameplaySceneChanged += UpdateAreaDisplay;
        UpdateAreaDisplay(m_sceneLoaderService.CurrentGameplaySceneData);
    }

    private void UpdateAreaDisplay(SSceneData sceneData)
    {
        m_sceneNameBinders.ForEach(x => x.BindAreaDisplay(sceneData));
    }
}
