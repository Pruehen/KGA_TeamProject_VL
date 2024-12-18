using EnumTypes;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : SceneSingleton<UIManager>
{
    [SerializeField] Image stamina;
    [SerializeField] Image healthPoint;
    [SerializeField] Image shildPoint;
    [SerializeField] Image skillPoint;
    [SerializeField] Image interactive;
    [SerializeField] Image bossHealth;

    [SerializeField] TextMeshProUGUI TMP_BulletText;
    [SerializeField] TextMeshProUGUI TMP_MeleeBulletText;

    [SerializeField] GameObject UI_MeleeBulletUI;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject tabUI;
    [SerializeField] GameObject EscUI;
    [SerializeField] GameObject blueChipUI;
    [SerializeField] GameObject pickBlueChip;
    [SerializeField] GameObject holdBlueChip;
    [SerializeField] GameObject outGamePassive;
    [SerializeField] GameObject checkUI;
    [SerializeField] Text questName;
    [SerializeField] Text questInfo;
    [SerializeField] Animator questAni;
    [SerializeField] Toggle questToggle;
    [SerializeField] Text questTimer;
    [SerializeField] Animator achievementAni;

    PlayerInstanteState _PlayerState;
    PlayerMaster _PlayerMaster;

    [SerializeField] Button pickButton;
    [SerializeField] Button holdButton;
    [SerializeField] GameObject escImage;

    [SerializeField] Text emeraldText;

    [SerializeField] List<PassiveUI> PassiveUIList;

    [SerializeField] BossCombatUI bossCombatUi;
    public BossCombatUI BossCombatUi => bossCombatUi;


    public void Init()
    {
        blueChipUI.SetActive(false);
        Init_PassiveUIList();

        if (_PlayerState != null)
        {
            _PlayerState.HealthRatioChanged += OnHealthRatioChanged;
            _PlayerState.ShildRatioChanged += OnShildRatioChanged;
            _PlayerState.StaminaRatioChanged += OnStaminaChanged;
            _PlayerState.BulletChanged += OnBulletChanged;
            _PlayerState.MeleeBulletChanged += OnMeleeBulletChanged;
            _PlayerState.SkillGaugeRatioChanged += OnSkillRatioChanged;
            _PlayerMaster.Mod.OnModChanged += OnMeleeModeChanged;
        }

        Command_Refresh_View();
    }

    void Command_Refresh_View()
    {
        _PlayerState.Refresh_Model();
        OnMeleeModeChanged(false);
    }

    private void OnDestroy()
    {
        InputManager.Instance.PropertyChanged -= OnInputPropertyChanged;
        if (_PlayerState != null)
        {
            _PlayerState.HealthRatioChanged -= OnHealthRatioChanged;
            _PlayerState.BulletChanged -= OnBulletChanged;
            _PlayerState.StaminaRatioChanged -= OnStaminaChanged;
            _PlayerState.SkillGaugeRatioChanged -= OnSkillRatioChanged;
            _PlayerMaster.Mod.OnModChanged -= OnMeleeModeChanged;
        }
    }

    private void Update()
    {
        QuestReturn();
        AchievementReturn();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (blueChipUI.activeSelf == true)
            {

                HoldButtonMove();
            }
            else
            {
                ReturnMainGame();

            }

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MainBlueChipList();
        }
    }

    private void ReturnMainGame()
    {
        CheckUIManager.Instance.CheckUiActive_OnClick(OutGame, "게임을 나가시겠습니까?");
    }

    private void Awake()
    {
        InputManager.Instance.PropertyChanged += OnInputPropertyChanged;
    }

    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(InputManager.Instance.IsInteractiveBtnClick):
                if (InputManager.Instance.IsInteractiveBtnClick == true)
                {
                    if (EventSystem.current.currentSelectedGameObject == null)
                    {
                        return;
                    }
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                    {
                        if (checkUI.activeSelf == false && holdBlueChip.activeSelf == true && pickBlueChip.activeSelf == false)
                        {
                            return;
                        }
                        else
                        {
                            Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
                            selectedButton.onClick.Invoke();
                        }

                    }



                }
                break;
        }
    }


    public void SetPlayerMaster(PlayerMaster pm)
    {
        _PlayerMaster = pm;
        _PlayerState = pm._PlayerInstanteState;
    }
    void Init_PassiveUIList()
    {
        if (_PlayerMaster != null)
        {
            List<PassiveID> usePasiveList = new List<PassiveID>();

            foreach (var item in _PlayerMaster._PlayerPassive.PassiveHashSet)
            {
                usePasiveList.Add(item);
            }
            usePasiveList.Sort();
            for (int i = 0; i < usePasiveList.Count; i++)
            {
                PassiveUIList[i].SetPassiveId(usePasiveList[i]);
            }
        }
    }

    public void Interactable(bool chest)
    {
        if (chest)
        {
            interactive.gameObject.SetActive(true);
        }
        else if (!chest)
        {
            interactive.gameObject.SetActive(false);
        }
    }

    public void OutGame()
    {
        GameManager.Instance.EndGame();
    }

    public void OnHealthRatioChanged(float value)
    {
        healthPoint.fillAmount = value;
    }
    public void OnShildRatioChanged(float value)
    {
        shildPoint.fillAmount = value;
    }
    public void OnStaminaChanged(float value)
    {
        stamina.fillAmount = value;
    }
    public void OnBulletChanged(int value, int maxValue)
    {
        TMP_BulletText.text = value + " / " + maxValue;
    }
    public void OnMeleeBulletChanged(int value, int maxValue)
    {
        TMP_MeleeBulletText.text = value + " / " + maxValue;
    }
    public void OnMeleeModeChanged(bool value)
    {
        UI_MeleeBulletUI.SetActive(value);
    }
    public void OnSkillRatioChanged(float value)
    {
        skillPoint.fillAmount = value;
    }

    //����
    public void BlueChipUI()
    {

        if (blueChipUI.activeSelf == true)
        {
            MainBlueChipList();
        }
        blueChipUI.SetActive(true);
        blueChipUI.GetComponent<BlueChipUIManager>().Init();
        TimeManager.instance.TimeStop();

        HoldButtonMove();

    }
    public void UpdateGoldInfoUI()
    {
        float playerEmerald = JsonDataManager.GetUserData().TryGetPlayData(out PlayData playData) ? playData.InGame_Gold : JsonDataManager.GetUserData().Gold;

        emeraldText.text = playerEmerald.ToString();
    }


    //FŰ�� ���� ���Ĩ�� �����ϸ� ȣ���Ǵ� �Լ�
    public void PickBUtton()
    {
        escImage.SetActive(true);
        EventSystem.current.SetSelectedGameObject(holdButton.gameObject);

    }


    //Esc�� ���� ��ü�� ����ϸ� ȣ��Ǵ� �Լ�
    public void HoldButtonMove()
    {
        escImage.SetActive(false);
        EventSystem.current.SetSelectedGameObject(pickButton.gameObject);

    }

    public void BkBlueChipUi()
    {
        blueChipUI.SetActive(false);
        TimeManager.instance.TimeStart();
    }

    public void MainBlueChipList()
    {

        if (blueChipUI.activeSelf == false)
        {
            blueChipUI.SetActive(true);
            outGamePassive.SetActive(true);
            pickBlueChip.SetActive(false);
            blueChipUI.GetComponent<BlueChipUIManager>().Init();

        }
        else if (holdBlueChip.activeSelf == true && pickBlueChip.activeSelf == false)
        {
            blueChipUI.SetActive(false);
            outGamePassive.SetActive(false);
            pickBlueChip.SetActive(true);
            TimeManager.instance.TimeStart();
        }

    }

    public void DrawQuestStartUi(string name, string discription)
    {
        questToggle.isOn = false;
        questAni.SetBool("Quest", true);
        questName.text = name;
        questInfo.text = discription;
        Debug.Log("Name :" + questName.text + "Info :" + questInfo.text);

    }

    public void DrawQuestUi(bool isHited)
    {
        if (isHited)
        {
            //실패
            Questfail();
        }
        else
        {
            //성공
            QuestClear();
        }

    }

    //성공
    public void QuestClear()
    {
        questToggle.isOn = true;
        questAni.SetBool("Quest", true);
    }

    //실패 
    public void Questfail()
    {
        questToggle.isOn = false;
        questAni.SetBool("QuestFail", true);
    }


    public void QuestReturn()
    {
        AnimatorStateInfo currentState = questAni.GetCurrentAnimatorStateInfo(0);

        if (currentState.IsName("OutQuestPanel"))
        {
            questAni.SetBool("Quest", false);
        }

        if (currentState.IsName("FailOutQuestPanel"))
        {
            questAni.SetBool(" QuestFail", false);
        }
    }

    //시간
    public void QuestTimerText(float timeCounter)
    {
        //Mathf.FloorToInt는 소수점 이하를 버리고 내림하여 가장 가까운 정수로 변환
        int minutes = Mathf.FloorToInt(timeCounter / 60);

        //Mathf.FloorToInt를 사용하여 소수점 이하를 버리고 내림
        int seconds = Mathf.FloorToInt(timeCounter % 60);

        // 분과 초를 00:00 형식의 문자열로 포맷팅하여 questTimer.text에 설정
        // {0:00} - 두 자리로 포맷된 minutes 값
        // {1:00} - 두 자리로 포맷된 seconds 값
        questTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //업적
    public void Achievement()
    {
        achievementAni.SetBool("Feat", true);
    }

    public void AchievementReturn()
    {
        AnimatorStateInfo currentState = achievementAni.GetCurrentAnimatorStateInfo(0);

        if (currentState.IsName("Achievement"))
        {
            achievementAni.SetBool("Feat", false);
        }
    }
}
