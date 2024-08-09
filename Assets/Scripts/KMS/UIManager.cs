using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SceneSingleton<UIManager>
{
    public Image stamina;
    public Image healthPoint;
    public Image skillPoint;
    public TextMeshProUGUI TMP_BulletText;
    public TextMeshProUGUI TMP_MeleeBulletText;
    [SerializeField] GameObject UI_MeleeBulletUI;
    public GameObject inGameUI;
    public GameObject tabUI;
    public GameObject EscUI;    

    [SerializeField] PlayerInstanteState PlayerState;
    [SerializeField] Slider healthSlider;
    
  
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
        if (healthSlider != null && PlayerState.maxHp != 0)
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
}

