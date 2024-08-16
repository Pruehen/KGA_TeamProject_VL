using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "StageListData", menuName = "Stage/StageList", order = 1)]
public class SO_StageList : ScriptableObject
{
    public SO_Stage[] StageData;
}
