using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] PlayerInstanteState PlayerState;    

    
  
    private void Start()
    {
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

    public void BlueChipUI()
    {
        blueChipUI.SetActive(true);
    }
}

