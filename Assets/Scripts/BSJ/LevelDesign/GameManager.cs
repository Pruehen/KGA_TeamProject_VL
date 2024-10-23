using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using Zenject.SpaceFighter;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public enum RewardType
{
    BlueChip,
    Currency,
}

public enum ResultSceneType
{
    Clear,
    Dead,
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

    public ResultSceneType ResultSceneType {get; private set;}

    private bool _isLoading = false;
    public bool IsLoading {
        set
        {
            _isLoading = value;
        } 
        get
        {
            return _isLoading;
        }
    }

    private IEnumerator SetEventSystemEnable(bool value)
    {
        yield return null;
        yield return null;
    }
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
        if (_isLoading)
        {
            return;
        }
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "mainGame" || sceneName == "ResultScene"|| sceneName == "MetaScene")
        {
            return;
        }
        UpdateCurrentQuest();
    }
    public void SetRewordType(RewardType rewordType)
    {
        _rewardType = rewordType;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BlockSceneChange = false;
        SceneManager.SetActiveScene(scene);
        if(mode == LoadSceneMode.Single)
        {
            IsLoading = false;
        }
        if (FindObjectsOfType<GameManager>().Length >= 2)
        {
            if (!_unique)
            {
                return;
            }
        }
        if (mode == LoadSceneMode.Single || mode == LoadSceneMode.Additive)
        {
            _unique = true;

            if (scene.name == "ResultScene" || scene.name == "MetaScene")
            {
                return;
            }
            if (scene.name == "mainGame")
            {
                SM.Instance.SetBGM(0);
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

            InitCurrentQuest();
        }
    }
    private void InitCurrentQuest()
    {
        if (_stageSystem.CurrentStageNum >= unexpectedquests.Length)
        {
            return;
        }
        if (unexpectedquests[_stageSystem.CurrentStageNum] != null)
        {
            _currentQuest = new Quest();
            _currentQuest.Init(unexpectedquests[_stageSystem.CurrentStageNum]);
            unexpectedquests[_stageSystem.CurrentStageNum].Init();
        }
    }
    private void UpdateCurrentQuest()
    {
        if (_stageSystem.CurrentStageNum >= unexpectedquests.Length)
        {
            return;
        }
        if (unexpectedquests[_stageSystem.CurrentStageNum] != null)
        {
            _currentQuest.DoUpdateQuest();
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
            if (_stageSystem.ChapterLength - 1 == _stageSystem.CurrentStageNum)
            {
                GameClear();
                return;
            }

            _stageSystem.Clear();
            OnGameClear?.Invoke();
            if (_currentQuest != null && unexpectedquests[_stageSystem.CurrentStageNum] != null)
            {
                _currentQuest?.IsCleared();
            }
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
        if (_isFirstStage) // spawn chest only when started game and has utility4 passive
        {
            _PlayerMaster._PlayerInstanteState.Passive_Utility4_Active();
        }

        RegisterEnemies();

        if (_PlayerMaster == null)
        {
            _PlayerMaster = PlayerMaster.Instance;
        }
        _PlayerMaster._PlayerInstanteState.OnDead += OnDead;
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




    public bool BlockSceneChange {get; set;} = false;
    public void StartGame()
    {
        if (BlockSceneChange)
        {
            return;
        }
        BlockSceneChange = true;
        if (_isLoading)
            return;
        _enemies.Clear();
        _currentQuest = null;
        EarnedCurrency = 0;

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
                LoadSceneAsync(playData.InGame_Stage.StageName, SceneManager.GetActiveScene(), LoadSceneMode.Additive, (ao, prevScene) =>
                {
                    StartCoroutine(UnloadPrevScene(prevScene));
                });
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
        LoadSceneAsync(randomStage.SceneName, SceneManager.GetActiveScene());
    }

    public void LoadScene(string sceneName)
    {
        if (_blockSceneChange)
        {
            return;
        }
        _blockSceneChange = true;
        DelayLoadScene(sceneName, 0f);
    }

    private void GameClear()
    {
        if(BlockSceneChange)
        {
            return;
        }
        BlockSceneChange = true;
        ResultSceneType = ResultSceneType.Clear;
        DelayLoadScene("ResultScene", 3f, () =>
        {
            ClearAndSave();
        });
    }
    private void OnDead(Combat self)
    {
        if(BlockSceneChange)
        {
            return;
        }
        BlockSceneChange = true;
        ResultSceneType = ResultSceneType.Dead;
        DelayLoadScene("ResultScene", 3f, () =>
        {
            ClearAndSave();
        });
    }
    public void EndGame()
    {
        if(BlockSceneChange)
        {
            return;
        }
        BlockSceneChange = true;

        var userData = JsonDataManager.GetUserData();
        JsonDataManager.GetUserData().SavePlayData_OnSceneEnter(new StageData(_stageSystem.CurrentStage.SceneName, _stageSystem.CurrentStageNum, _rewardType, _stageSystem.CurrentStage));
        LoadSceneAsync("mainGame", SceneManager.GetActiveScene());
    }
    public void LoadMainScene()
    {
        if(BlockSceneChange)
        {
            return;
        }
        BlockSceneChange = true;
        LoadSceneAsync("mainGame", SceneManager.GetActiveScene());
    }



    private IEnumerator CheckAddtiveLoadingend(AsyncOperation ao, Scene prevScene, LoadSceneMode mode, Action<AsyncOperation, Scene> onComplete)
    {
        if (ao == null)
        {
            yield break;
        }
        bool isAlmostDone = false;
        while (true)
        {
            isAlmostDone = ao.progress >= 0.9f;
            if (isAlmostDone)
            {
                break;
            }
            yield return null;
        }
        ao.allowSceneActivation = true;
    }
    private IEnumerator UnloadPrevScene(Scene prevScene)
    {
        float unloadTime = 1.5f;
        GameObject[] rootObjects = prevScene.GetRootGameObjects();
        foreach (var obj in rootObjects)
        {
            if(obj.TryGetComponent(out FaderSceneMainUi sceneUiFader))
            {
                sceneUiFader.OnSceneUnloaded(unloadTime - .5f);
            }
        }
        yield return new WaitForSeconds(unloadTime);
        AsyncOperation ao = SceneManager.UnloadSceneAsync(prevScene);
        ao.completed += (ao) =>
        {
            IsLoading = false;
            BlockSceneChange = false;
        };
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
        LoadSceneAsync(nextStage.SceneName, SceneManager.GetActiveScene());
    }
    public AsyncOperation LoadSceneAsync(string sceneName, Scene activeScene, LoadSceneMode mode = LoadSceneMode.Single, Action<AsyncOperation, Scene> onComplete = null)
    {
        if (_isLoading)
        {
            return null;
        }
        IsLoading = true;

        Destroy(GameObject.FindObjectOfType<XROrigin>().gameObject);
        Destroy(GameObject.FindObjectOfType<XRInteractionManager>().gameObject);

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName, mode);
        if(onComplete != null)
        {
            ao.allowSceneActivation = false;
            ao.completed += (ao) => onComplete(ao, activeScene);
        }
        StartCoroutine(CheckAddtiveLoadingend(ao, SceneManager.GetActiveScene(), mode, onComplete));
        return ao;
    }
    public void DelayLoadScene(string sceneName, float delay = 0f, Action delayEnd = null)
    {
        StartCoroutine(DelayLoadSceneCoroutine(sceneName, delay, delayEnd));
    }
    private IEnumerator DelayLoadSceneCoroutine(string sceneName, float delay = 0f, Action delayEnd = null)
    {
        yield return new WaitForSeconds(delay);
        delayEnd?.Invoke();
        LoadSceneAsync(sceneName, SceneManager.GetActiveScene(), LoadSceneMode.Additive, (ao2, prevScene) =>
        {
            StartCoroutine(UnloadPrevScene(prevScene));
        });
    }


    public int EarnedCurrency {get; set;}





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
        _PlayerMaster.transform.position = NextStageObjects.transform.position - (NextStageObjects.transform.forward * 3f);
    }




    public string MetaVersePlayerName {get; set;}
    public bool IsMetaVerseServer {get; set;} = true;
}
