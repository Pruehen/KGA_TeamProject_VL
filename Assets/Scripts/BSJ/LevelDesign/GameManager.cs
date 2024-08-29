using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public enum RewardType
{
    BlueChip,
    Currency,
}

public class GameManager : SceneSingleton<GameManager>
{
    [SerializeField] private SO_ChapterData _chapterData;

    public NextStageObjects NextStageObjects;

    private bool _unique = false;
    private RewardType _rewardType;

    private int _currentLevel;

    private Action OnClear;

    public PlayerMaster _PlayerMaster { get; private set; }

    public List<GameObject> _enemies = new List<GameObject>();

    public Action OnGameClear;

    public SO_Quest[] unexpectedquests;
    private Quest _currentQuest = new Quest();

    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length >= 2)
        {
            Destroy(gameObject);
            return;
        }
        transform.SetParent(null);
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "mainGame")
        {
            return;
        }
        if (unexpectedquests[GetCurrentLevelIndex()] != null)
        {
            _currentQuest.CheckConditionOnUpdate();
        }
    }
    public void SetRewordType(RewardType rewordType)
    {
        _rewardType = rewordType;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (FindObjectsOfType<GameManager>().Length >= 2)
        {
            if (!_unique)
            {
                return;
            }
        }
        if (mode == LoadSceneMode.Single)
        {
            _unique = true;

            if (scene.name == "mainGame")
            {
                return;
            }

            _PlayerMaster = FindAnyObjectByType<PlayerMaster>();
            NextStageObjects = FindAnyObjectByType<NextStageObjects>();
           
            if (_PlayerMaster == null)
            {
                return;
            }
            else
                Assert.IsNotNull(_PlayerMaster);


            Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

            foreach (Enemy e in enemies)
            {
                _enemies.Add(e.gameObject);
            }

            foreach (Enemy enemy in enemies)
            {
                enemy.RegisterOnDead(OnEnemyDead);
            }

            NextStageObjects.Init(_rewardType);


            if (unexpectedquests[GetCurrentLevelIndex()] != null)
            {
                _currentQuest.Init(unexpectedquests[GetCurrentLevelIndex()]);
                unexpectedquests[GetCurrentLevelIndex()].Init();
            }
        }
    }

    private void SetStageQuests()
    {
        SO_RandomQuestSetData randomQuestSet = _chapterData.RandomQuestsData;
        int count = 0;
        unexpectedquests = new SO_Quest[_chapterData.ChapterData.Length];
        for (int i = 0; i < unexpectedquests.Length; i++)
        {
            SO_Quest r = randomQuestSet.TryGetRandomQuest();
            if (r != null)
            {
                count++;
            }
            unexpectedquests[i] = r;
        }

        if (count == 0)
        {
            unexpectedquests[UnityEngine.Random.Range(0, unexpectedquests.Length)] = randomQuestSet.GetRandomQuest();
        }
    }

    public void StartChapter()
    {
        int stageNum = 0;
        SO_Stage randomStage = GetRandomItem(_chapterData.ChapterData[stageNum].StageData);

        AsyncOperation ao = SceneManager.LoadSceneAsync(randomStage.SceneName);
        ao.allowSceneActivation = true;

        SetStageQuests();
    }

    public void LoadNextStage()
    {
        _enemies.Clear();
        JsonDataManager.GetUserData().SavePlayData_OnSceneExit(_PlayerMaster._PlayerInstanteState, _PlayerMaster._PlayerEquipBlueChip);
        SO_Stage randomStage = GetRandomItem(_chapterData.ChapterData[GetCurrentLevelIndex()].StageData);

        AsyncOperation ao = SceneManager.LoadSceneAsync(randomStage.SceneName);
        ao.allowSceneActivation = true;
    }

    private int GetCurrentLevelIndex()
    {
        return _currentLevel++ % _chapterData.ChapterData.Length;
    }

    private T GetRandomItem<T>(T[] array)
    {
        int r = UnityEngine.Random.Range(0, array.Length);
        return array[r];
    }

    private void OnEnemyDead(GameObject enemy)
    {
        _enemies.Remove(enemy);
        if (_enemies.Count == 0)
        {
            OnGameClear?.Invoke();
        }
    }

    public bool IsCurrentUnexpectedQuestCleared()
    {
        if (GetCurrentLevelIndex() >= unexpectedquests.Length)
        {
            Debug.LogError("OutOfLength");
            return false;
        }
        if (unexpectedquests[GetCurrentLevelIndex()] == null)
        {

            return false;
        }

        return _currentQuest.IsCleared();
    }
}
