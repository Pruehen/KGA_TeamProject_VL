using UnityEngine;

[CreateAssetMenu(fileName = "QuestNoModChangeData", menuName = "Quests/Condition/QuestNoModChange", order = 0)]
public class QuestNoModChange : SO_Quest
{
    private bool isModChanged;

    public override void Init()
    {
        base.Init();
        isModChanged = false;

        PlayerMaster.Instance.Mod.OnModChanged += OnModChanged;

        UIManager.Instance.DrawQuestStartUi(Name, Discription);
    }
    public override void DoUpdate()
    {
        if (IsQuestEnd)
            return;
        if(isModChanged)
        {
            QuestFail();
        }
    }
    public override bool IsCleared()
    {
        if (isModChanged)
        {
            return false;
        }
        QuestClear();
        return true;
    }

    private void OnModChanged(bool mod)
    {
        isModChanged = true;
    }

    public override void OnEnd()
    {
        PlayerMaster.Instance.Mod.OnModChanged -= OnModChanged;
    }
}
