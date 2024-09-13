using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using static SO_Stage;

public enum RewardType
{
    BlueChip,
    Currency,
}

public class GameManager : SceneSingleton<GameManager>
{
    [SerializeField] private GameObject _blueChipChest;
    public NextStageObjects NextStageObjects;

    private bool _unique = false;
    private bool _init = false;
    private RewardType _rewardType;

    public PlayerMaster _PlayerMaster { get; private set; }

    public List<EnemyBase> _enemies = new List<EnemyBase>();

    public SO_Quest[] unexpectedquests;
    private Quest _currentQuest;

    [SerializeField] private StageSystem _stageSystem;

    public Action OnGameClear;

    [SerializeField] private SO_RandomQuestSetData _randomQuestsData;

    private bool _isLoading = false;
    private bool _isFirstStage;

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
            _currentQuest.DoUpdateQuest();
        }
    }
    public void SetRewordType(RewardType rewordType)
    {
        _rewardType = rewordType;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _isLoading = false;
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
                SM.Instance.SetBGM(0);
                Debug.Log("if (scene.name == mainGame)");
                return;
            }

            if (_init == false)
            {
                _init = true;
            }

            _PlayerMaster = PlayerMaster.Instance;
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
                _currentQuest = new Quest();
                _currentQuest.Init(unexpectedquests[_stageSystem.CurrentStageNum]);
                unexpectedquests[_stageSystem.CurrentStageNum].Init();
            }
        }
    }
    private void SetStageQuests()
    {
        SO_RandomQuestSetData randomQuestSet = _randomQuestsData;
        int count = 0;
        unexpectedquests = new SO_Quest[_stageSystem.ChapterLength - 1];
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

        JsonDataManager.GetUserData().SavePlayData_Quest(unexpectedquests);
    }
    public void StartGame()
    {
        if (_isLoading)
            return;
        _enemies.Clear();
        _currentQuest = null;

        //Load
        if (JsonDataManager.GetUserData().TryGetPlayData(out PlayData playData))
        {
            if (playData.InGame_Stage != null)
            {
                _stageSystem.LoadChapter(playData.InGame_Stage.StageNum, playData.InGame_Stage.Stage);
                unexpectedquests = playData.InGame_Quest;

                if (playData.InGame_Stage.StageNum == 0)
                {
                    _isFirstStage = true;
                }

                SM.Instance.SetBGM((int)_stageSystem.CurrentStage.sceneType);
                LoadSceneAsync(playData.InGame_Stage.StageName);
                return;
            }
        }
        _isFirstStage = true;
        _stageSystem.ResetStageSystem();
        //Start New
        _stageSystem.StartChapter();
        SO_Stage randomStage = _stageSystem.GetCurrentRandomStage();
        SM.Instance.SetBGM((int)randomStage.sceneType);
        SetStageQuests();
        JsonDataManager.GetUserData().SavePlayData_OnSceneEnter(new StageData(randomStage.SceneName, _stageSystem.CurrentStageNum, _rewardType, _stageSystem.CurrentStage));
        LoadSceneAsync(randomStage.SceneName);
    }
    private T GetRandomItem<T>(T[] array)
    {
        int r = UnityEngine.Random.Range(0, array.Length);
        return array[r];
    }
    private void OnEnemyDead(EnemyBase enemy)
    {
        _enemies.Remove(enemy);
        if (_enemies.Count == 0)
        {
            if(_stageSystem.ChapterLength - 1 == _stageSystem.CurrentStageNum)
            {
                StartCoroutine(GameClear());
                return;
            }

            _stageSystem.Clear();
            OnGameClear?.Invoke();
            if(_currentQuest != null && unexpectedquests[_stageSystem.CurrentStageNum] != null)
            {
                _currentQuest?.IsCleared();
            }
        }
    }

    private IEnumerator GameClear()
    {
        yield return new WaitForSeconds(3f);
        ClearAndSave();
        LoadMainScene();
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
        if (_isFirstStage) // spawn chest only when started game and has utility4 passive
        {
            _PlayerMaster._PlayerInstanteState.Passive_Utility4_Active();
        }

        RegisterEnemies();

        if(_PlayerMaster == null)
        {
            _PlayerMaster = PlayerMaster.Instance;
        }
        _PlayerMaster._PlayerInstanteState.OnDead += OnDead;
    }
    public void OnPlayerDead()
    {
        StartCoroutine(GameClear());
    }
    private void RegisterEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.OnDeadWithSelf = null;
        }
        _enemies.Clear();

        EnemyBase[] enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);

        foreach (EnemyBase e in enemies)
        {
            _enemies.Add(e);
        }

        foreach (EnemyBase enemy in enemies)
        {
            enemy.OnDeadWithSelf += OnEnemyDead;
        }
    }
    private void OnDead(Combat self)
    {
        ClearAndSave();
        LoadMainScene();
    }

    private void ClearAndSave()
    {
        _PlayerMaster._PlayerInstanteState.OnDead -= OnDead;
        UserData userData = JsonDataManager.GetUserData();
        userData.TryGetPlayData(out PlayData playData);

        userData.ClearAndSaveUserData();
        if (_stageSystem.CurrentStage == null)
        {
            Debug.LogWarning("CurrentStage is Null");
            return;
        }
    }
    public void EndGame()
    {
        var userData = JsonDataManager.GetUserData();

        JsonDataManager.GetUserData().SavePlayData_OnSceneEnter(new StageData(_stageSystem.CurrentStage.SceneName, _stageSystem.CurrentStageNum, _rewardType, _stageSystem.CurrentStage));

        LoadMainScene();
    }
    public void LoadNextStage()
    {
        if (_isLoading)
            return;
        
        _isFirstStage = false;

        SO_Stage nextStage = _stageSystem.GetNextRandomStage();
        if (_stageSystem.CurrentStageNum == 0)
        {
            SetStageQuests();
        }
        JsonDataManager.GetUserData().SavePlayData_OnSceneExit(_PlayerMaster._PlayerInstanteState, _PlayerMaster._PlayerEquipBlueChip);
        SM.Instance.SetBGM((int)nextStage.sceneType);
        Debug.Log("SM.Instance.SetBGM((int)nextStage.sceneType);");
        LoadSceneAsync(nextStage.SceneName);
    }
    public void LoadSceneAsync(string sceneName)
    {
        if (_isLoading)
        {
            return;
        }
        _isLoading = true;
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.allowSceneActivation = true;
    }
    public void LoadMainScene()
    {
        LoadSceneAsync("mainGame");
    }
    public void KillAll()
    {
        List<EnemyBase> enemyList = new List<EnemyBase>(_enemies);
        foreach (var enemy in enemyList)
        {
            enemy.Hit(9999f);
        }
    }
    public void SpawnBluechipChest()
    {
        Transform trf = _PlayerMaster.transform;
        Vector3 pos = trf.position;
        pos += trf.forward + trf.right;
        pos.y = 0f;
        Instantiate(_blueChipChest, pos, Quaternion.identity);
    }

    public void TPToDoor()
    {
        _PlayerMaster.transform.position = NextStageObjects.transform.position + -NextStageObjects.transform.forward;
    }
}
