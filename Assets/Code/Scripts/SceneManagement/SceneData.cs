using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Scene Data")]
public class SceneData: ScriptableObject
{
    [SerializeField]
    private SSceneData m_sceneData;

    public SSceneData Data { get { return m_sceneData; } }
}

[Serializable]
public struct SSceneData
{
    public ESceneTypes SceneType;
    public EScenes SceneName;
    public EGameState SceneState;
    public string ParentAreaName;
    public string AreaName;
}
