using System;
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

    private bool _init = false;
    private RewardType _rewardType;

    private int _currentLevel;

    private Action OnClear;

    public PlayerMaster _PlayerMaster { get; private set; }

    public Enemy[] _enemies;

    public Action OnGameClear;

    public int _deadCount = 0;

    public SO_Quest[] unexpectedquests;
    private Quest _currentQuest = new Quest();
    public SO_RandomQuestSetData _randomQuestSet;



    private void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length >= 2)
        {
            Destroy(gameObject);
            return;
        }
        transform.SetParent(null);
        DontDestroyOnLoad(this);

        NextStageObjects = FindAnyObjectByType<NextStageObjects>();
        Assert.IsNotNull(NextStageObjects);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        OnSceneLoaded(new Scene(), LoadSceneMode.Single);
    }

    private void Update()
    {
        if (unexpectedquests[_currentLevel] != null)
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
            if (!_init)
            {
                return;
            }
        }
        if (mode == LoadSceneMode.Single)
        {
            if (_init == false)
            {
                _init = true;
                SetStageQuests();
            }
            else
            {
            }


            _PlayerMaster = FindAnyObjectByType<PlayerMaster>();
            NextStageObjects = FindAnyObjectByType<NextStageObjects>();
            Assert.IsNotNull(_PlayerMaster);

            _enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

            foreach (Enemy enemy in _enemies)
            {
                enemy.RegisterOnDead(OnEnemyDead);
            }

            NextStageObjects.Init(_rewardType);


            if (unexpectedquests[_currentLevel] != null)
            {
                _currentQuest.Init(unexpectedquests[_currentLevel]);
                unexpectedquests[_currentLevel].Init();
            }
        }
    }

    private void SetStageQuests()
    {
        int count = 0;
        unexpectedquests = new SO_Quest[_chapterData.ChapterData.Length];
        for (int i = 0; i < unexpectedquests.Length; i++)
        {
            SO_Quest r = _randomQuestSet.TryGetRandomQuest();
            if (r != null)
            {
                count++;
            }
            unexpectedquests[i] = r;
        }

        if (count == 0)
        {
            unexpectedquests[UnityEngine.Random.Range(0, unexpectedquests.Length)] = _randomQuestSet.GetRandomQuest();
        }
    }

    public void LoadNextStage()
    {
        JsonDataManager.GetUserData().SavePlayData_OnSceneExit(_PlayerMaster._PlayerInstanteState, _PlayerMaster._PlayerEquipBlueChip);
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
        if (_enemies.Length == _deadCount)
        {
            OnGameClear?.Invoke();
        }
    }

    public bool IsCurrentUnexpectedQuestCleared()
    {
        if (_currentLevel >= unexpectedquests.Length)
        {
            Debug.LogError("OutOfLength");
            return false;
        }
        if (unexpectedquests[_currentLevel] == null)
        {

            return false;
        }

        return _currentQuest.IsCleared();
    }
}
