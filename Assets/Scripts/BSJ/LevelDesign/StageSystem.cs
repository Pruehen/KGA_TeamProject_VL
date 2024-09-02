using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageSystem
{
    [SerializeField] private SO_ChapterData _chapterData;
    private int _currentStageNum;
    public int CurrentStageNum { get { return _currentStageNum; } private set { _currentStageNum = value; } }

    public int ChapterLength { get { return _chapterData.ChapterData.Length; } }

    private SO_Stage _currentStage;
    public SO_Stage CurrentStage { get { return _currentStage; } private set { _currentStage = value; } }

    private Action OnChapterStart;
    private Action<int> OnLevelStart;


    bool _initChapter = false;
    public void LoadChapter(int stageNum, SO_Stage stage)
    {
        StartChapter();

        CurrentStageNum = stageNum;
        CurrentStage = stage;
    }

    public void StartChapter()
    {
        if (_initChapter)
            return;
        _initChapter = true;
        
        OnChapterStart?.Invoke();
    }
    public SO_Stage GetCurrentRandomStage()
    {
        List<SO_Stage> availables = _chapterData.ChapterData[CurrentStageNum].GetAvailableStages();
        var randomStage = availables[UnityEngine.Random.Range(0, availables.Count)];
        CurrentStage = randomStage;
        return randomStage;

    }
    internal SO_Stage GetNextRandomStage()
    {
        AddLevelIndex();
        return GetCurrentRandomStage();
    }
    private void AddLevelIndex()
    {
        _currentStageNum++;
        CurrentStageNum = _currentStageNum % _chapterData.ChapterData.Length;
    }

    public void Clear()
    {
        CurrentStage.Cleared = true;
    }

}
