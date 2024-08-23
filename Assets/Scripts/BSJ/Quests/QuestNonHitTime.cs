using UnityEngine;

[CreateAssetMenu(fileName = "QuestNonHitTimeData", menuName = "Quests/Condition/QuestNonHitTime", order = 0)]
public class QuestNonHitTime : SO_Quest
{
    public float LimitTime;

    private float timeCounter;
    private bool isHited;

    public override void Init()
    {
        timeCounter = 0f;
        isHited = false;

        PlayerMaster.Instance._PlayerInstanteState.OnDamaged += OnDamaged;
    }
    public override bool CheckConditionOnUpdate()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter > LimitTime)
        {
            return false;
        }
        if(isHited)
        {
            return false;
        }
        return true;
    }

    private void OnDamaged()
    {
        isHited = true;
    }

    public override void OnEnd()
    {
        PlayerMaster.Instance._PlayerInstanteState.OnDamaged -= OnDamaged;
    }
}
