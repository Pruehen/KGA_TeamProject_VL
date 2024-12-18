using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ChapterData", menuName = "Stage/Chapter", order = 1)]
public class SO_ChapterData : ScriptableObject
{
    public SO_RandomStageList[] ChapterData;
    public SO_RandomQuestSetData RandomQuestsData;

    public void ResetChapter()
    {
        foreach (SO_RandomStageList stageList in ChapterData)
        {
            stageList.ResetStageList();
        }
    }
}
