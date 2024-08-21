using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "StageListData", menuName = "Stage/StageList", order = 1)]
public class SO_RandomStageList : ScriptableObject
{
    public SO_Stage[] StageData;
}
