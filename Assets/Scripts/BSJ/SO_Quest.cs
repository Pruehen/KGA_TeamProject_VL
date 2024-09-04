using System;
using UnityEngine;

public enum QuestDfficurty
{
    Easy,
    Normal,
    Hard,
}

public class SO_Quest : ScriptableObject
{
    public QuestDfficurty Difficurty;
    public string Name;
    public string Discription;
    public bool IsQuestEnd = false;

    public virtual void Init()
    {
        IsQuestEnd = false;
    }

    public virtual void DoUpdate()
    {
    }

    public virtual bool IsCleared()
    {
        return false;
    }

    protected virtual void QuestFail()
    {
        if (IsQuestEnd)
        {
            return;
        }
        IsQuestEnd = true;
        UIManager.Instance.Questfail();
    }
    protected virtual void QuestClear()
    {
        if (IsQuestEnd)
        {
            return;
        }
        IsQuestEnd = true;
        UIManager.Instance.QuestClear();
    }

    public virtual void OnEnd()
    {
        return;
    }
}

[Serializable]
public class Quest
{
    SO_Quest _questCondition;

    public void Init(SO_Quest quest)
    {
        _questCondition = quest;
        _questCondition.Init();
    }

    public void DoUpdateQuest()
    {
        if (_questCondition == null)
        {
            Debug.LogWarning("씬 로드시 초기화가 업데이트보다 느림");
            return;
        }
        _questCondition.DoUpdate();
    }

    public bool IsCleared()
    {
        return _questCondition.IsCleared();
    }

    public void OnEnd()
    {
        _questCondition.OnEnd();
    }
}