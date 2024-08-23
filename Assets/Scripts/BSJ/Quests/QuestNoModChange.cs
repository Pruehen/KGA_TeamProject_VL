using UnityEngine;

[CreateAssetMenu(fileName = "QuestNoModChangeData", menuName = "Quests/Condition/QuestNoModChange", order = 0)]
public class QuestNoModChange : SO_Quest
{
    private bool isModChanged;

    public override void Init()
    {
        isModChanged = false;

        PlayerMaster.Instance._PlayerInstanteState.OnMeleeModeChanged += OnModChanged;
    }
    public override bool CheckConditionOnUpdate()
    {
        if(isModChanged)
        {
            return false;
        }
        return true;
    }

    private void OnModChanged(bool mod)
    {
        isModChanged = true;
    }

    public override void OnEnd()
    {
        PlayerMaster.Instance._PlayerInstanteState.OnMeleeModeChanged -= OnModChanged;
    }
}
