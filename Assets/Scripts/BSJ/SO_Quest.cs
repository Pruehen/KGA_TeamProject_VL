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
        if(_questCondition == null)
        {
            Debug.LogWarning("씬 로드시 초기화가 업데이트보다 느림");
            return ;
        }
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