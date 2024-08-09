using TMPro;
using UI.Extension;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SceneSingleton<UIManager>
{
    public Image stamina;
    public Image healthPoint;
    public Image skillPoint;
    public TextMeshProUGUI bullet;
    public GameObject inGameUI;
    public GameObject tabUI;
    public GameObject EscUI;    

    [SerializeField] PlayerInstanteState PlayerState;    

    
  
    private void Start()
    {
        if (PlayerState != null)
        {
            PlayerState.HealthChanged += OnHealthChanged;
            PlayerState.StaminaChanged += OnStaminaChanged;
            PlayerState.BulletChanged += OnBulletChanged;
            PlayerState.SkillGaugeChanged += ONSkillGaugeChanged;
        }
        UpdateHealthView();
        UpdateStaminaView();
        UpdateBulletView();
        UpdateSkillGaugeView();
    }
    private void OnDestroy()
    {
        if (PlayerState != null)
        {
            PlayerState.HealthChanged -= OnHealthChanged;
            PlayerState.BulletChanged -= OnBulletChanged;
            PlayerState.StaminaChanged -= OnStaminaChanged;
            PlayerState.SkillGaugeChanged -= ONSkillGaugeChanged;
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
        if (bullet != null && PlayerState.bullets != 0)
        {
            stamina.fillAmount = PlayerState.stamina / PlayerState.MaxStamina;
            bullet.text = PlayerState.bullets + " + " + PlayerState.maxBullets;
        }
    }
    public void UpdateSkillGaugeView()
    {
        if (PlayerState == null)
            return;
        if (skillPoint != null)
        {
            skillPoint.fillAmount = PlayerState.skillGauge / PlayerState.MaxskillGauge;
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
    public void ONSkillGaugeChanged()
    {
        UpdateSkillGaugeView();

    }



}
