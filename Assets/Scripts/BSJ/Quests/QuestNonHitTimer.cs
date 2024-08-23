using UnityEngine;

[CreateAssetMenu(fileName = "QuestConditionData", menuName = "Quests/QuestConditionData", order = 0)]
public class QuestNonHitTimer : SO_Quest
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
