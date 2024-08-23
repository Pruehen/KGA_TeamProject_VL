using UnityEngine;

public class SO_Quest : ScriptableObject
{
    public virtual void Init()
    { }

    public virtual bool CheckConditionOnUpdate()
    {
        return false;
    }

    public virtual void OnEnd()
    {
        return;
    }
}

public class Quest
{
    SO_Quest _questCondition;

    public bool _cleared;
    public bool _start;

    public void Init(SO_Quest quest)
    {
        _questCondition = quest;
        _questCondition.Init();
    }

    public void CheckConditionOnUpdate()
    {
        _cleared = _questCondition.CheckConditionOnUpdate();
    }

    public void StartQuest()
    {
        _start = true;
    }

    public bool IsCleared()
    {
        return _cleared;
    }

    public void OnEnd()
    {
    }
}