using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public enum RewardType
{
    Currency,
    BlueChip
}

public class GameManager : SceneSingleton<GameManager>
{
    [SerializeField] private SO_ChapterData _chapterData;

    public NextStageObjects NextStageObjects;

    private bool _init = false;
    private RewardType _rewardType;

    private int _currentLevel;

    private Action OnClear;

    public PlayerMaster _PlayerMaster {  get; private set; }

    public Enemy[] _enemies;

    public Action OnGameClear;

    public int _deadCount = 0;

    private void Awake()
    {
        if(FindObjectsOfType<GameManager>().Length >= 2)
        {
            Destroy(gameObject);
        }
        transform.SetParent(null);
        DontDestroyOnLoad(this);

        NextStageObjects = FindAnyObjectByType<NextStageObjects>();
        Assert.IsNotNull(NextStageObjects);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        _PlayerMaster = FindAnyObjectByType<PlayerMaster>();
        Assert.IsNotNull(_PlayerMaster);

        _enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in _enemies)
        {
            enemy.RegisterOnDead(OnEnemyDead);
        }

        NextStageObjects.Init(_rewardType);
    }

    public void SpawnReword(RewardType rewordType)
    {
        switch (rewordType)
        {
            case RewardType.Currency:
                
                // 골드 스폰
                break;
            case RewardType.BlueChip:
                // 블루칩 스폰
                break;
        }
    }

    public void SetRewordType(RewardType rewordType)
    {
        _rewardType = rewordType;
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
        SO_Stage randomStage = GetRandomItem(_chapterData.ChapterData[_currentLevel++ % _chapterData.ChapterData.Length].StageData);

        AsyncOperation ao = SceneManager.LoadSceneAsync(randomStage.SceneName);
        ao.allowSceneActivation = true;
    }

    private T GetRandomItem<T>(T[] array)
    {
        int r = UnityEngine.Random.Range(0, array.Length);
        return array[r];
    }

    private void OnEnemyDead()
    {
        _deadCount++;
        if(_enemies.Length == _deadCount)
        {
            OnGameClear?.Invoke();
        }
    }
}
