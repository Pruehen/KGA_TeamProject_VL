using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "StageListData", menuName = "Stage/StageList", order = 1)]
public class SO_RandomStageList : ScriptableObject
{
    public SO_Stage[] StageData;

    public List<SO_Stage> GetAvailableStages()
    {
        List<SO_Stage> availables = new List<SO_Stage>();
        foreach (SO_Stage stage in StageData)
        {
            if(stage.Cleared == false )
                availables.Add(stage);
        }

        if (availables.Count == 0)
        {
            foreach (SO_Stage stage in StageData)
            {
                stage.Cleared = false;
                availables.Add(stage);
            }
        }

        return availables;
    }

    public void ResetStageList()
    {
        foreach(SO_Stage stageData in StageData)
        {
            stageData.ResetStageData();
        }
    }
}
