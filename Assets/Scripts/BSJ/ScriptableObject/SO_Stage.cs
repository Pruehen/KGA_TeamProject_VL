using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "StageData", menuName = "Stage/Stage", order = 0)]
public class SO_Stage : ScriptableObject
{
    public string SceneName;

    public bool Cleared;

    [SerializeField]
    public SceneType sceneType;

    public enum SceneType
    {
        Lobby,
        Normal,
        Boss
    }

    public void ResetStageData()
    {
        Cleared = false;
    }
}
