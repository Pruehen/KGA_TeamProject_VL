using System;
using UnityEngine;

public class PlayerInstanteState : MonoBehaviour
{
    public float hp { get; private set; }
    public float Shield { get; private set; }
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
    [SerializeField] float staminaRecoveryDelay;
    float staminaRecoveryDelayValue = 0;

    [SerializeField] float MaxskillGauge = 100;
    [SerializeField] int maxBullets = 50;
    [SerializeField] int maxMeleeBullets = 50;

    [SerializeField] float attackPower;
    [SerializeField] float skillPower;
    public float GetDmg()
    {         
        return attackPower;
    }

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
        staminaRecoveryDelayValue += Time.deltaTime;
        if (staminaRecoveryDelayValue >= staminaRecoveryDelay)
        {
            StaminaAutoRecovery();
        }
    }

    //���¹̳� �Ҹ� 
    public bool TryStaminaConsumption(float power)
    {
        if (stamina > power)
        {
            stamina -= power;    
            staminaRecoveryDelayValue = 0;
            UpdateStamina();
            return true;
        }
        else
            return false;
    }

    //���¹̳� �ڵ� ȸ��
    public void StaminaAutoRecovery()
    {
        if (stamina < MaxStamina)
        {
            stamina += staminaRecoverySpeed * Time.deltaTime;
        }

        if(stamina > MaxStamina)
        {
            stamina = MaxStamina;
        }
     
        UpdateStamina();
    }

    public void Hit(float dmg)
    {        
        if(Shield > 0)
        {
            Shield -= dmg;
            if (Shield <= 0)
            {
                Shield = 0;
            }
            UpdateShild();
            return;
        }

        if(hp > 0)
        {
            hp -= dmg;
            if (hp <= 0)
            {
                hp = 0;
                IsDead = true;                
            }
            UpdateHealth();
        }                
    }

    public void ChangeHp(float value)
    {
        hp += value;
        if(hp > maxHp)
        {
            hp = maxHp;
        }
        if(hp < 0)
        {
            hp = 1;
        }

        UpdateHealth();
    }

    public void ChangeShield(float value)
    {
        Shield += value;
        if(Shield > maxHp)
        {
            Shield = maxHp;
        }
        if (Shield < 0)
        {
            Shield = 0;
        }
        UpdateShild();
    }

    //źȯ ȹ��
    public void AcquireBullets(int _bullets)
    {
        bullets += _bullets;
        if(bullets > maxBullets)
        {
            bullets = maxBullets;
        }

        UpdateBullet();
    }

    //źȯ �Ҹ�
    public void BulletConsumption()
    {
        if (bullets != 0)
            bullets--;
        else
        {
            Debug.Log("ź�� ����");
            return;
        }
        UpdateBullet();
    }
    //źȯ ȹ��
    public void AcquireBullets_Melee(int _bullets)
    {
        meleeBullets += _bullets;
        if(meleeBullets > maxBullets)
        {
            meleeBullets = maxBullets;
        }

        UpdateBullet_Melee();
    }

    //źȯ �Ҹ�
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
        ShildRatioChanged?.Invoke(Shield / maxHp);
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
