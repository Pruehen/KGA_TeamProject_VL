using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
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
    [SerializeField] Slider healthSlider;
    private void Start()
    {
        if (PlayerState != null)
        {
            PlayerState.HealthChanged += OnHealthChanged;
            PlayerState.StaminaChanged += OnStaminaChanged;
        }
        UpdateHealthView();
        UpdateStaminaView();
    }
    private void OnDestroy()
    {
        if (PlayerState != null)
        {
            PlayerState.HealthChanged -= OnHealthChanged;
        }
    }
    public void Damage(float amount)
    {
        PlayerState?.Hit(amount);
    }
    public void Reset()
    {
        PlayerState?.Restore();
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
    public void OnHealthChanged()
    {
        UpdateHealthView();
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

    public void OnStaminaChanged()
    {
        UpdateStaminaView();
    }
   
}
