using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RewordType
{
    Gold,
    BlueChip
}

public class GameManager : SceneSingleton<GameManager>
{
    [SerializeField] private SO_StageList[] _stageBlocks;
    private bool _init = false;
    private RewordType _rewordType;

    private int _currentLevel;

    private Action OnClear;

    private void Awake()
    {
        if(FindObjectsOfType<GameManager>().Length >= 2)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnClear += HandleClear;
    }

    public void SpawnReword(RewordType rewordType)
    {
        switch (rewordType)
        {
            case RewordType.Gold:
                // ��� ����
                break;
            case RewordType.BlueChip:
                // ���Ĩ ����
                break;
        }
    }

    public void SetRewordType(RewordType rewordType)
    {
        _rewordType = rewordType;
    }

    private void HandleClear()
    {
        SpawnReword(_rewordType);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
        {
            if (_init == false)
            {
                _init = true;
            }
            else
            {

            }
        }
    }

    public void LoadNextStage()
    {
        SO_Stage randomStage = GetRandomItem(_stageBlocks[_currentLevel++ % _stageBlocks.Length].StageData);

        AsyncOperation ao = SceneManager.LoadSceneAsync(randomStage.SceneName);
        ao.allowSceneActivation = true;
    }

    private T GetRandomItem<T>(T[] array)
    {
        int r = UnityEngine.Random.Range(0, array.Length);
        return array[r];
    }

}
