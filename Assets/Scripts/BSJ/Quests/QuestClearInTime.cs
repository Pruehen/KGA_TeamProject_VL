using UnityEngine;

[CreateAssetMenu(fileName = "QuestClearInTimeData", menuName = "Quests/Condition/QuestClearInTime", order = 0)]
public class QuestClearInTime : SO_Quest
{
    public float LimitTime = 0f;
    private float timeCounter = 0f;

    public override void Init()
    {
        timeCounter = 0f;
    }
    public override bool CheckConditionOnUpdate()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter > LimitTime)
        {
            return false;
        }
        return true;
    }
}
