using System;
using UnityEngine;

[RequireComponent(typeof(PlayerEquipBlueChip))]
public class PlayerInstanteState : MonoBehaviour
{
    public float hp { get; private set; }
    public float Shild { get; private set; }
    public float stamina { get; private set; }
    public int bullets { get; private set; }
    public int meleeBullets { get; private set; }
    public float skillGauge { get; private set; }
    public bool IsAbsorptState { get; set; }

    bool _isMeleeMode;
    public bool IsMeleeMode
    {
        get { return _isMeleeMode; }
        set
        {
            _isMeleeMode = value;
            OnMeleeModeChanged?.Invoke(_isMeleeMode);
        }
    }

    public bool IsDead { get; private set; }

    [SerializeField] float maxHp;    
    [SerializeField] float MaxStamina;
    [SerializeField] float staminaRecoverySpeed;

    [SerializeField] float MaxskillGauge = 100;
    [SerializeField] int maxBullets = 50;
    [SerializeField] int maxMeleeBullets = 50;

    [SerializeField] float attackPower;
    public float GetAttackPower() { return attackPower; }

    [SerializeField] float moveSpeed;
    public float GetMoveSpeed() { return moveSpeed; }


    public Action<float> HealthRatioChanged;
    public Action<float> ShildRatioChanged;
    public Action<float> StaminaRatioChanged;
    public Action<int, int> BulletChanged;
    public Action<int, int> MeleeBulletChanged;
    public Action<float> SkillGaugeRatioChanged;
    public Action<bool> OnMeleeModeChanged;

    private void Awake()
    {
        Restore();
        UIManager.Instance.setPlayer(this);
    }
    private void Start()
    {        
        //UIManager.Instance.UpdateStamina(stamina, MaxStamina);
        UpdateHealth();
        UpdateStamina();
        UpdateSkillGauge();
    }

    private void Update()
    {
        
        StaminaAutoRecovery();
    }

    //스태미나 소모 
    public bool TryStaminaConsumption(float power)
    {

        if (stamina > power)
        {
            stamina -= power;            
            UpdateStamina();
            return true;
        }
        else
            return false;
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
        bullets += _bullets;
        if(bullets > maxBullets)
        {
            bullets = maxBullets;
        }

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
    //탄환 획득
    public void AcquireBullets_Melee(int _bullets)
    {
        meleeBullets += _bullets;
        if(meleeBullets > maxBullets)
        {
            meleeBullets = maxBullets;
        }

        UpdateBullet_Melee();
    }

    //탄환 소모
    public bool TryBulletConsumption_Melee(int value)
    {
        if(meleeBullets >= value)
        {
            meleeBullets -= value;
            UpdateBullet_Melee();
            return true;
        }
        else
        {
            return false;
        }
    }


    public void SkillGaugeRecovery(float value)
    {
        skillGauge += value;

        if(skillGauge > MaxskillGauge)
        {
            skillGauge = MaxskillGauge;
        }

        UpdateSkillGauge();
    }
    public bool TryUseSkillGauge(float value)
    {
        if(skillGauge >= value)
        {
            skillGauge -= value;
            UpdateSkillGauge();
            return true;
        }
        else
        {
            return false;
        }                
    }


    void Restore()
    {
        hp = maxHp;
        IsDead = false;
        stamina = MaxStamina;
        skillGauge = 0;
        bullets = maxBullets / 3;
    }

    public void Refresh_Model()
    {
        UpdateHealth();
        UpdateShild();
        UpdateStamina();
        UpdateBullet();
        UpdateBullet_Melee();
        UpdateSkillGauge();
    }

    public void UpdateHealth()
    {
        HealthRatioChanged?.Invoke(hp / maxHp);
    }
    public void UpdateShild()
    {
        ShildRatioChanged?.Invoke(Shild / maxHp);
    }
    public void UpdateStamina()
    {
        StaminaRatioChanged?.Invoke(stamina / MaxStamina);
    }
    public void UpdateBullet()
    {
        BulletChanged?.Invoke(bullets, maxBullets);
    }
    public void UpdateBullet_Melee()
    {
        MeleeBulletChanged?.Invoke(meleeBullets, maxMeleeBullets);
    }
    public void UpdateSkillGauge()
    {
        SkillGaugeRatioChanged?.Invoke(skillGauge / MaxskillGauge);
    }    
}
