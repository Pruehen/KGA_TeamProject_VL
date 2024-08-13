using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : SceneSingleton<UIManager>
{
    [SerializeField] Image stamina;
    [SerializeField] Image healthPoint;
    [SerializeField] Image skillPoint;
    [SerializeField] Image interactive;
    [SerializeField] TextMeshProUGUI TMP_BulletText;
    [SerializeField] TextMeshProUGUI TMP_MeleeBulletText;
    [SerializeField] GameObject UI_MeleeBulletUI;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject tabUI;
    [SerializeField] GameObject EscUI;
    [SerializeField] GameObject blueChipUI;

    [SerializeField] PlayerInstanteState PlayerState;

    [SerializeField] Button pickButton;
    [SerializeField] Button holdButton;
    [SerializeField] GameObject escImage;


    private void Start()
    {
        if (PlayerState != null)
        {
            PlayerState.HealthChanged += OnHealthChanged;
            PlayerState.StaminaChanged += OnStaminaChanged;
            PlayerState.BulletChanged += OnBulletChanged;
            PlayerState.MeleeBulletChanged += OnMeleeBulletChanged;
            PlayerState.OnMeleeModeChanged += OnMeleeModeChanged;
          
        }
        UpdateHealthView();
        UpdateStaminaView();
        UpdateBulletView();
        UpdateMeleeBulletView();
        OnMeleeModeChanged(false);
    }
    private void OnDestroy()
    {
        if (PlayerState != null)
        {
            PlayerState.HealthChanged -= OnHealthChanged;
            PlayerState.BulletChanged -= OnBulletChanged;
            PlayerState.StaminaChanged -= OnStaminaChanged;
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
                    if (blueChipUI.activeSelf == true)
                    {
                        Button selectedButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
                        selectedButton.onClick.Invoke();

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

    public void UpdateHealthView()
    {
        if (PlayerState == null)
            return;
        if (healthPoint != null && PlayerState.maxHp != 0)
        {
            healthPoint.fillAmount = PlayerState.hp / PlayerState.maxHp;
        }
    }

    public void UpdateStaminaView()
    {
        if (PlayerState == null)
            return;
        if (stamina != null && PlayerState.stamina != 0)
        {
            stamina.fillAmount = PlayerState.stamina / PlayerState.MaxStamina;
        }
    }
    public void UpdateBulletView()
    {
        if (PlayerState == null)
            return;
        if (TMP_BulletText != null)
        {
            TMP_BulletText.text = PlayerState.bullets + " / " + PlayerState.maxBullets;
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

    public void UpdateMeleeBulletView()
    {
        if (PlayerState == null)
            return;
        if (TMP_MeleeBulletText != null)
        {
            TMP_MeleeBulletText.text = PlayerState.meleeBullets + " / " + PlayerState.maxBullets;
        }
    }

    public void OnHealthChanged()
    {
        UpdateHealthView();
    }
    public void OnStaminaChanged()
    {
        UpdateStaminaView();
    }
    public void OnBulletChanged()
    {
        UpdateBulletView();
    }
    public void OnMeleeBulletChanged()
    {
        UpdateMeleeBulletView();
    }
    public void OnMeleeModeChanged(bool value)
    {
        UI_MeleeBulletUI.SetActive(value);
    }

    public void BlueChipUI()
    {
        blueChipUI.SetActive(true);
        TimeManager.instance.TimeStop();

        HoldButtonMove();

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
}

