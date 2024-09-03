using UnityEngine;

[CreateAssetMenu(fileName = "QuestNonHitTimeData", menuName = "Quests/Condition/QuestNonHitTime", order = 0)]
public class QuestNonHit : SO_Quest
{
    private bool isHited;

    public override void Init()
    {
        base.Init();
        isHited = false;

        PlayerMaster.Instance._PlayerInstanteState.OnDamaged += OnDamaged;

        UIManager.Instance.DrawQuestStartUi(Name, Discription);
    }
    public override void DoUpdate()
    {
    }

    public override bool IsCleared()
    {
        if (isHited)
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
