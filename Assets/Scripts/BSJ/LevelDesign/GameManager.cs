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
    public NextStageObjects NextStageObjects;

    private bool _unique = false;
    private bool _init = false;
    private RewardType _rewardType;

    public PlayerMaster _PlayerMaster { get; private set; }

    public List<Enemy> _enemies = new List<Enemy>();

    public SO_Quest[] unexpectedquests;
    private Quest _currentQuest = new Quest();

    [SerializeField] private StageSystem _stageSystem;

    public Action OnGameClear;

    [SerializeField] private SO_RandomQuestSetData _randomQuestsData;

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
        if (unexpectedquests[_stageSystem.CurrentStageNum] != null)
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
        Debug.Log("�� �ε��");
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
            if (unexpectedquests[_stageSystem.CurrentStageNum] != null)
            {
                _currentQuest.Init(unexpectedquests[_stageSystem.CurrentStageNum]);
                unexpectedquests[_stageSystem.CurrentStageNum].Init();
            }
        }
    }


    private void SetStageQuests()
    {
        SO_RandomQuestSetData randomQuestSet = _randomQuestsData;
        int count = 0;
        unexpectedquests = new SO_Quest[_stageSystem.ChapterLength];
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

    bool _initChapter = false;

    public void StartGame()
    {
        _enemies.Clear();

        //Load
        if (JsonDataManager.GetUserData().TryGetPlayData(out PlayData playData))
        {
            if (playData.InGame_Stage != null)
            {
                _stageSystem.LoadChapter(playData.InGame_Stage.StageNum, playData.InGame_Stage.Stage);
                unexpectedquests = playData.InGame_Quest;

                
                LoadSceneAsync(playData.InGame_Stage.StageName);
                return;
            }
        }

        //Start New
        _stageSystem.StartChapter();
        SO_Stage randomStage = _stageSystem.GetNextStage();
        SetStageQuests();
        JsonDataManager.GetUserData().SavePlayData_OnChapterEnter(unexpectedquests);
        JsonDataManager.GetUserData().SavePlayData_OnSceneEnter(new StageData(randomStage.SceneName, _stageSystem.CurrentStageNum, _rewardType,_stageSystem.CurrentStage));
        LoadSceneAsync(randomStage.SceneName);
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
            _stageSystem.Clear();
            OnGameClear?.Invoke();
        }
    }

    public bool IsCurrentUnexpectedQuestCleared()
    {
        if (_stageSystem.CurrentStageNum >= unexpectedquests.Length)
        {
            Debug.LogError("OutOfLength");
            return false;
        }
        if (unexpectedquests[_stageSystem.CurrentStageNum] == null)
        {

            return false;
        }

        return _currentQuest.IsCleared();
    }

    public void OnPlayerSpawn()
    {
        RegisterEnemies();

        _PlayerMaster._PlayerInstanteState.OnDead += OnDead;
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

    private void OnDead()
    {
        UserData userData = JsonDataManager.GetUserData();
        _PlayerMaster._PlayerInstanteState.OnDead -= OnDead;
        userData.TryGetPlayData(out PlayData playData);
        userData.AddGold(playData.InGame_Gold - userData.Gold);
        userData.ClearAndSaveUserData();
        LoadMainScene();
    }

    public void EndGame()
    {
        var userData = JsonDataManager.GetUserData();
        if (userData.TryGetPlayData(out PlayData playData))
        {
            userData.AddGold(playData.InGame_Gold - userData.Gold);
        }
        else
        {
            Debug.LogWarning("�� ���� �÷��̵�����");
        }
        LoadMainScene();
    }


    public void LoadNextStage()
    {
        SO_Stage nextStage = _stageSystem.GetNextStage();
        JsonDataManager.GetUserData().SavePlayData_OnSceneExit(_PlayerMaster._PlayerInstanteState,_PlayerMaster._PlayerEquipBlueChip);
        LoadSceneAsync(nextStage.SceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = true;
    }
    public void LoadMainScene()
    {
        LoadSceneAsync("mainGame");
    }
}
