using System;
using System.Collections;
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
    private bool _init = false;
    private RewardType _rewardType;

    private int _currentLevel;

    private Action OnClear;

    public PlayerMaster _PlayerMaster { get; private set; }

    public List<Enemy> _enemies = new List<Enemy>();

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
        if (SceneManager.GetActiveScene().name == "mainGame")
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
        Debug.Log("¾À ·ÎµåµÊ");
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

            if (_init == false)
            {
                _init = true;
            }

            _PlayerMaster = FindAnyObjectByType<PlayerMaster>();
            NextStageObjects = FindAnyObjectByType<NextStageObjects>();

            if (_PlayerMaster == null)
            {
                return;
            }
            else
                Assert.IsNotNull(_PlayerMaster);
            
            NextStageObjects.Init(_rewardType);
            if (unexpectedquests[GetCurrentLevelIndex()] != null)
            {
                _currentQuest.Init(unexpectedquests[GetCurrentLevelIndex()]);
                unexpectedquests[GetCurrentLevelIndex()].Init();
            }
        }
    }

    private void RegisterEnemies()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy e in enemies)
        {
            _enemies.Add(e);
        }

        foreach (Enemy enemy in enemies)
        {
            enemy.OnDeadWithSelf += OnEnemyDead;
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
        _enemies.Clear();

        if(JsonDataManager.GetUserData().TryGetPlayData(out PlayData playData))
        {
            if (playData.InGame_Stage != null)
            {
                _currentLevel = playData.InGame_Stage.StageNum;
                unexpectedquests = playData.InGame_Quest;

                AsyncOperation ao2 = SceneManager.LoadSceneAsync(playData.InGame_Stage.StageName);
                ao2.allowSceneActivation = true;

                return;
            }
        }
        SO_Stage randomStage = GetRandomItem(_chapterData.ChapterData[_currentLevel].StageData);
        SetStageQuests();
        JsonDataManager.GetUserData().SavePlayData_OnChapterEnter(unexpectedquests);
        JsonDataManager.GetUserData().SavePlayData_OnSceneEnter(new StageData(randomStage.SceneName, _currentLevel, _rewardType));

        AsyncOperation ao = SceneManager.LoadSceneAsync(randomStage.SceneName);
        ao.allowSceneActivation = true;
    }

    public void LoadNextStage()
    {
        _enemies.Clear();
        JsonDataManager.GetUserData().SavePlayData_OnSceneExit(_PlayerMaster._PlayerInstanteState, _PlayerMaster._PlayerEquipBlueChip);
        SO_Stage randomStage = GetRandomItem(_chapterData.ChapterData[GetCurrentLevelIndex()].StageData);

        AddLevelIndex();


        JsonDataManager.GetUserData().SavePlayData_OnSceneEnter(new StageData(randomStage.SceneName, _currentLevel, _rewardType));

        AsyncOperation ao = SceneManager.LoadSceneAsync(randomStage.SceneName);
        ao.allowSceneActivation = true;
    }
    public void LoadMainScene()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync("mainGame");
        ao.allowSceneActivation = true;
    }

    private int GetCurrentLevelIndex()
    {
        return _currentLevel % _chapterData.ChapterData.Length;
    }
    private void AddLevelIndex()
    {
        _currentLevel++;
    }

    private T GetRandomItem<T>(T[] array)
    {
        int r = UnityEngine.Random.Range(0, array.Length);
        return array[r];
    }

    private void OnEnemyDead(Enemy enemy)
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

    public void OnPlayerSpawn()
    {
        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null)
            {
                enemy.OnDeadWithSelf -= OnEnemyDead;
            }
        }
        _enemies.Clear();

        RegisterEnemies();

        _PlayerMaster._PlayerInstanteState.OnDead += OnDead;
    }

    private void OnDead()
    {
        UserData userData = JsonDataManager.GetUserData();
        _PlayerMaster._PlayerInstanteState.OnDead -= OnDead;
        userData.TryGetPlayData(out PlayData playData);
        userData.AddGold(playData.InGame_Gold - userData.Gold);
        userData.ClearAndSaveUserData();
        LoadMainScene();
    }
}
