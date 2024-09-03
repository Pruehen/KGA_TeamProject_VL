using UnityEngine;

[CreateAssetMenu(fileName = "QuestClearInTimeData", menuName = "Quests/Condition/QuestClearInTime", order = 0)]
public class QuestClearInTime : SO_Quest
{
    public float LimitTime = 0f;
    private float timeCounter = 0f;

    public override void Init()
    {
        base.Init();
        timeCounter = 0f;

        UIManager.Instance.DrawQuestStartUi(Name, Discription);
    }
    public override bool CheckConditionOnUpdate()
    {
        timeCounter += Time.deltaTime;
        UIManager.Instance.QuestTimerText(timeCounter);
        if (timeCounter > LimitTime)
        {
            return false;
        }
        return true;
    }
}
