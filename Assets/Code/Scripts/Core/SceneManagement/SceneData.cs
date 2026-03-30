using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Scene Data")]
public class SceneData: ScriptableObject
{
    [SerializeField]
    private SSceneData m_sceneData;

    public SSceneData Data => m_sceneData;
}

[Serializable]
public struct SSceneData
{
    [SerializeField] private ESceneTypes m_sceneType;
    [SerializeField] private EScenes m_sceneName;
    [SerializeField] private EGameStates m_sceneState;
    [SerializeField] private string m_parentAreaName;
    [SerializeField] private string m_areaName;

    public ESceneTypes SceneType { get { return m_sceneType; } }
    public EScenes SceneName { get { return m_sceneName; } }
    public EGameStates SceneState { get { return m_sceneState; } }
    public string ParentAreaName { get { return m_parentAreaName; } }
    public string AreaName { get { return m_areaName; } }
}
