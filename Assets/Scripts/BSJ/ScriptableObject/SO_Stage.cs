using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "StageData", menuName = "Stage/Stage", order = 0)]
public class SO_Stage : ScriptableObject
{
    public string SceneName;

    public bool Cleared;

    public void ResetStageData()
    {
        Cleared = false;
    }
}
