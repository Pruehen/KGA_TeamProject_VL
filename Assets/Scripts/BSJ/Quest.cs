using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestCondition
{
    public bool CheckCondition();
}

public class Quest : MonoBehaviour
{
    public bool _cleared;
    public bool _start;

    IQuestCondition _questCondition; 

    private void Update()
    {
        if(!_start)
        {
            return;
        }
        if( _cleared )
        {
            return ;
        }
        if(_questCondition.CheckCondition())
        {
            _cleared = true;
        }
    }

    public void StartQuest()
    {
        //_start;
    }

    public bool IsCleared()
    {
        return _cleared;
    }
}
