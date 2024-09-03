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
    public bool IsQuestUpdatable = false;


    public virtual void Init()
    { }

    public virtual void DoUpdate()
    {
    }

    public virtual bool IsCleared()
    {
        return false;
    }

    protected virtual void QuestFail()
    {
        UIManager.Instance.Questfail();
    }
    protected virtual void QuestClear()
    {
        UIManager.Instance.QuestClear();
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

    public void DoUpdateQuest()
    {
        if(_questCondition == null)
        {
            Debug.LogWarning("씬 로드시 초기화가 업데이트보다 느림");
            return ;
        }
        _questCondition.DoUpdate();
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