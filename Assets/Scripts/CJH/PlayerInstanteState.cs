using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Zenject.SpaceFighter;

public class PlayerInstanteState : MonoBehaviour
{
    public float hp;
    public float stamina;
    public int bullets;


    public bool IsDead { get; private set; }

    [SerializeField]
    public float maxHp;

    [SerializeField]
    public float MaxStamina;

    [SerializeField]
    private float staminaRecoverySpeed;


    [SerializeField]
    public int maxBullets;

    // 현재 보유 탄환량 변수 추가
    // 최대 보유 가능 탄환량 변수 추가 (시리얼라이즈필드)
    // 탄환 소모 메서드 추가
    // 탄환 획득 메서드 추가



    public event Action HealthChanged;
    public event Action StaminaChanged;
    public event Action BulletChanged;

    private void Awake()
    {
        UIManager.Instance.setPlayer(this);
    }
    private void Start()
    {
        IsDead = false;
        stamina = MaxStamina;

        //UIManager.Instance.UpdateStamina(stamina, MaxStamina);
        UpdateHealth();
        UpdateStamina();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StaminaConsumption(10);
            Hit(10);
        }

        StaminaAutoRecovery();
    }

    //스태미나 소모 
    public void StaminaConsumption(float power)
    {
        if (stamina > power)
        {
            stamina -= power;
            //UIManager.Instance.UpdateStamina(stamina, MaxStamina);
            UpdateStamina();
        }
        else
            return;

    }

    //스태미나 자동 회복
    public void StaminaAutoRecovery()
    {
        if (stamina < MaxStamina)
        {
            stamina += staminaRecoverySpeed * Time.deltaTime;

            //UIManager.Instance.UpdateStamina(stamina, MaxStamina);
            UpdateStamina();
        }
        else if (stamina > MaxStamina)
        {
            stamina = MaxStamina;
            return;
        }
    }

    public void Hit(float dmg)
    {
        //dmg만큼 체력 감소
        if (hp > 0)
        {
            hp -= dmg;

        }
        else
            return;

        //체력이 0이 될 경우 IsDead를 true로.
        if (hp == 0)
        {
            IsDead = true;
            Debug.Log("죽음");
        }
        //체력 수치와 UI 연동.

        //UIManager.Instance.UpdatehealthPoint(hp, maxHp);
        UpdateHealth();

    }

    //탄환 획득
    public void AcquireBullets(int _bullets)
    {
        if (bullets < maxBullets)
        {
            bullets += _bullets;
            Debug.Log("bullets : " + bullets);
        }
        else
            return;
        UpdateBullet();
    }

    //탄환 소모
    public void BulletConsumption()
    {
        if (bullets != 0)
            bullets--;
        else
        {
            Debug.Log("탄알 없음");
            return;
        }
        UpdateBullet();
    }


    public void Restore()
    {
        hp = maxHp;
    }
    public void UpdateHealth()
    {
        HealthChanged?.Invoke();
    }
    public void UpdateStamina()
    {
        StaminaChanged?.Invoke();
    }
    public void UpdateBullet()
    {
        StaminaChanged?.Invoke();
    }

}








