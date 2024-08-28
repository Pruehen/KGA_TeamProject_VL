using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using EnumTypes;
using System;
using Unity.VisualScripting;

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
  

    PlayerInstanteState _PlayerState;
    PlayerMaster _PlayerMaster;

    [SerializeField] Button pickButton;
    [SerializeField] Button holdButton;
    [SerializeField] GameObject escImage;

    [SerializeField] Text emeraldText;
        
    [SerializeField] List<PassiveUI> PassiveUIList;

    private void Start()
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

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MainBlueChipList();
        }
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
                    if (blueChipUI.activeSelf == true && pickBlueChip.activeSelf == true)
                    {

                        Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
                        selectedButton.onClick.Invoke();

                    }
                    else
                    {
                        return;
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

    //½ÃÀÛ
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
        float playerEmerald = JsonDataManager.GetUserData().TryGetPlayData(out PlayData playData) ? playData.InGame_Gold : -999999999f;

        emeraldText.text = playerEmerald.ToString();
    }

  
    //FÅ°¸¦ ´­·¯ ºí·çÄ¨À» ¼±ÅÃÇÏ¸é È£­ŒµÇ´Â ÇÔ¼ö
    public void PickBUtton()
    {
        escImage.SetActive(true);
        EventSystem.current.SetSelectedGameObject(holdButton.gameObject);

    }


    //Esc¸¦ ´­·¯ ±³Ã¼¸¦ Ãë¼ÒÇÏ¸é È£ÃâµÇ´Â ÇÔ¼ö
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