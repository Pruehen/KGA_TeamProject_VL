using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EnumTypes;

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

    [SerializeField] PlayerInstanteState PlayerState;

    [SerializeField] Button pickButton;
    [SerializeField] Button holdButton;
    [SerializeField] GameObject escImage;

    private void Start()
    {
        blueChipUI.SetActive(false);

        if (PlayerState != null)
        {
            PlayerState.HealthRatioChanged += OnHealthRatioChanged;
            PlayerState.ShildRatioChanged += OnShildRatioChanged;
            PlayerState.StaminaRatioChanged += OnStaminaChanged;
            PlayerState.BulletChanged += OnBulletChanged;
            PlayerState.MeleeBulletChanged += OnMeleeBulletChanged;
            PlayerState.OnMeleeModeChanged += OnMeleeModeChanged;

        }

        Command_Refresh_View();
    }

    void Command_Refresh_View()
    {
        PlayerState.Refresh_Model();
        OnMeleeModeChanged(false);
    }

    private void OnDestroy()
    {
        if (PlayerState != null)
        {
            PlayerState.HealthRatioChanged -= OnHealthRatioChanged;
            PlayerState.BulletChanged -= OnBulletChanged;
            PlayerState.StaminaRatioChanged -= OnStaminaChanged;
            PlayerState.MeleeBulletChanged -= OnMeleeBulletChanged;
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

    public void setPlayer(PlayerInstanteState player)
    {
        PlayerState = player;
    }
    public void Damage(float amount)
    {
        PlayerState?.Hit(amount);
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

            pickBlueChip.SetActive(false);
            blueChipUI.GetComponent<BlueChipUIManager>().Init();
        }
        else
        {
            blueChipUI.SetActive(false);

            pickBlueChip.SetActive(true);

        }
       
    }

}