using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using EnumTypes;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class UIManager : SceneSingleton<UIManager>
{
    [SerializeField] Image stamina;
    [SerializeField] Image healthPoint;
    [SerializeField] Image shildPoint;
    [SerializeField] Image skillPoint;
    [SerializeField] Image interactive;

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
    [SerializeField] GameObject CheckUI;
    

      PlayerInstanteState _PlayerState;
    PlayerMaster _PlayerMaster;

    [SerializeField] Button pickButton;
    [SerializeField] Button holdButton;
    [SerializeField] GameObject escImage;

    [SerializeField] Text emeraldText;
        
    [SerializeField] List<PassiveUI> PassiveUIList;

    private void Start()
    {
        emeraldText.text = JsonDataManager.GetUserData().Gold.ToString();
        blueChipUI.SetActive(false);
        Init_PassiveUIList();

        if (_PlayerState != null)
        {
            _PlayerState.HealthRatioChanged += OnHealthRatioChanged;
            _PlayerState.ShildRatioChanged += OnShildRatioChanged;
            _PlayerState.StaminaRatioChanged += OnStaminaChanged;
            _PlayerState.BulletChanged += OnBulletChanged;
            _PlayerState.MeleeBulletChanged += OnMeleeBulletChanged;
            _PlayerState.OnMeleeModeChanged += OnMeleeModeChanged;
            _PlayerState.SkillGaugeRatioChanged += OnSkillRatioChanged;
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
        if (_PlayerState != null)
        {
            _PlayerState.HealthRatioChanged -= OnHealthRatioChanged;
            _PlayerState.BulletChanged -= OnBulletChanged;
            _PlayerState.StaminaRatioChanged -= OnStaminaChanged;
            _PlayerState.MeleeBulletChanged -= OnMeleeBulletChanged;
            _PlayerState.SkillGaugeRatioChanged -= OnSkillRatioChanged;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (blueChipUI.activeSelf == true)
            {
                HoldButtonMove();
            }

            ReturnMainGame();

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MainBlueChipList();
        }
    }

    private void ReturnMainGame()
    {
        Debug.Log("뒤로각;");
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
                    if (EventSystem.current.currentSelectedGameObject?.GetComponent<Button>())
                    {
                        if (CheckUI.activeSelf == false && holdBlueChip.activeSelf == true && pickBlueChip.activeSelf == false)
                        {
                            return;
                        }
                        else {
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
        if(_PlayerMaster != null)
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
        SceneManager.LoadScene("mainGame");
    
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

    //시작
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
    public void GoldInfoUI(float amount)
    {
        Debug.Log("amount" + amount);
        float playerEmerald = float.Parse(emeraldText.text);
        playerEmerald += amount;

        emeraldText.text = playerEmerald.ToString();

    }

  
    //F키를 눌러 블루칩을 선택하면 호춯되는 함수
    public void PickBUtton()
    {
        escImage.SetActive(true);
        EventSystem.current.SetSelectedGameObject(holdButton.gameObject);

    }


    //Esc를 눌러 교체를 취소하면 호출되는 함수
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

}