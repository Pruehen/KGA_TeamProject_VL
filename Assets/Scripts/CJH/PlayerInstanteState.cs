using System;
using UnityEngine;
using EnumTypes;
public class PlayerInstanteState : MonoBehaviour
{
    PlayerMaster _PlayerMaster;

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
    public float GetDmg(float coefficient)
    {
        float baseDmg = attackPower * coefficient;
        float dmgGain = 1;
        if(true)//차지 공격일 경우
        {
            int level = _PlayerMaster.GetBlueChipLevel(BlueChipID.근거리1);
            if (level > 0)
            {
                baseDmg += ((hp + Shield) * JsonDataManager.GetBlueChipData(BlueChipID.근거리1).Level_VelueList[level][0]) * 0.01f;
            }
        }
        if(true)//원거리 평타, 근거리 평타일 경우
        {
            if(_PlayerMaster._PlayerBuff.blueChip4_Buff_NextHitAddDmg.TryDequeue(out float addDmgGain))
            {
                dmgGain += addDmgGain;
                Debug.Log("피해증가 버프 소모");
            }
        }

        return baseDmg * dmgGain;
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
        _PlayerMaster = GetComponent<PlayerMaster>();

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

    //스태미나 소모 
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

    //스태미나 자동 회복
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
