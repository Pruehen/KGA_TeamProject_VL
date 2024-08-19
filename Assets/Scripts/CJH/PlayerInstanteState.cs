using EnumTypes;
using System;
using System.Collections.Generic;
using UnityEngine;
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
    public float AttackSpeed { get => attackSpeed; private set => attackSpeed = value; }

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
    float skillGaugeRecoveryRestTime = 0;
    [SerializeField] int maxBullets = 50;
    [SerializeField] int maxMeleeBullets = 50;

    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float attackPower;
    [SerializeField] float skillPower;
    public float GetDmg(PlayerAttackKind type, int combo)
    {
        float baseDmg = attackPower;// * coefficient;
        float dmgGain = 1;
        if (type == PlayerAttackKind.MeleeChargedAttack)//차지 공격일 경우
        {
            int level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Melee1);
            if (level > 0)
            {
                baseDmg += ((hp + Shield) * JsonDataManager.GetBlueChipData(BlueChipID.Melee1).Level_VelueList[level][0]) * 0.01f;
            }
        }
        if (type == PlayerAttackKind.MeleeNormalAttack || type == PlayerAttackKind.RangeNormalAttack)//원거리 평타, 근거리 평타일 경우
        {
            if (_PlayerMaster._PlayerBuff.blueChip4_Buff_NextHitAddDmg.TryDequeue(out float addDmgGain))
            {
                dmgGain += addDmgGain;
                Debug.Log("피해증가 버프 소모");
            }
            int blueChip7Level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Generic2);
            if (blueChip7Level > 0)
            {
                float addDmg = JsonDataManager.GetBlueChipData(BlueChipID.Generic2).Level_VelueList[blueChip7Level][1] * 0.01f;
                dmgGain += addDmg;
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

    [SerializeField] SO_Player _playerStatData;
    private void Awake()
    {
        _PlayerMaster = GetComponent<PlayerMaster>();        
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

        int blueChip8Level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Generic3);
        if (blueChip8Level > 0)
        {
            skillGaugeRecoveryRestTime += Time.deltaTime;
            if (skillGaugeRecoveryRestTime > JsonDataManager.GetBlueChipData(BlueChipID.Generic3).Level_VelueList[blueChip8Level][4])
            {
                UseSkillGauge(9999);
            }
        }
    }

    public void Init(PlayerPassive playerPassive)
    {
        maxHp = _playerStatData.maxHp;
        MaxStamina = _playerStatData.MaxStamina;
        staminaRecoverySpeed = _playerStatData.staminaRecoverySpeed;
        staminaRecoveryDelay = _playerStatData.staminaRecoveryDelay;

        MaxskillGauge = _playerStatData.MaxskillGauge;
        maxBullets = _playerStatData.maxBullets;
        maxMeleeBullets = _playerStatData.maxMeleeBullets;

        attackSpeed = _playerStatData.attackSpeed;
        attackPower = _playerStatData.attackPower;
        skillPower = _playerStatData.skillPower;

        moveSpeed = _playerStatData.moveSpeed;

        hp = maxHp;
        IsDead = false;
        stamina = MaxStamina;
        skillGauge = 0;
        bullets = maxBullets / 3;
        AttackSpeed = 1;
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

        if (stamina > MaxStamina)
        {
            stamina = MaxStamina;
        }

        UpdateStamina();
    }

    public void Hit(float dmg)
    {
        if (Shield > 0)
        {
            Shield -= dmg;
            if (Shield <= 0)
            {
                Shield = 0;
            }
            UpdateShild();
            return;
        }

        if (hp > 0)
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
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        if (hp < 0)
        {
            hp = 1;
        }

        UpdateHealth();
    }

    public void ChangeShield(float value)
    {
        Shield += value;
        if (Shield > maxHp)
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
        if (bullets > maxBullets)
        {
            bullets = maxBullets;
        }
        UpdateBullet();
    }

    //탄환 소모
    public void BulletConsumption()
    {
        int blueChip7Level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Generic2);
        int cost = (blueChip7Level > 0) ? (int)JsonDataManager.GetBlueChipData(BlueChipID.Generic2).Level_VelueList[blueChip7Level][0] : 1;

        bullets -= cost;
        if (bullets < 0)
            bullets = 0;
        UpdateBullet();
    }
    //근접탄 획득
    public void AcquireBullets_Melee(int _bullets)
    {
        meleeBullets += _bullets;
        if (meleeBullets > maxBullets)
        {
            meleeBullets = maxBullets;
        }
        UpdateBullet_Melee();
    }

    //근접탄 소모
    public void BulletConsumption_Melee()
    {
        int blueChip7Level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Generic2);
        int cost = (blueChip7Level > 0) ? (int)JsonDataManager.GetBlueChipData(BlueChipID.Generic2).Level_VelueList[blueChip7Level][2] : 1;

        meleeBullets -= cost;
        if (meleeBullets < 0)
            meleeBullets = 0;
        UpdateBullet_Melee();
    }


    public void SkillGaugeRecovery(float value)
    {
        skillGauge += value;

        if (skillGauge > MaxskillGauge)
        {
            skillGauge = MaxskillGauge;
        }

        skillGaugeRecoveryRestTime = 0;
        UpdateSkillGauge();
    }

    public void UseSkillGauge(float value)
    {
        skillGauge -= value;
        if (skillGauge < 0)
            skillGauge = 0;

        UpdateSkillGauge();
    }
    public void TryUseSkillGauge2()
    {
        float quotient = Mathf.Floor(skillGauge / 100);
        float remainder = skillGauge % 100;
        skillGauge = remainder;
        UpdateSkillGauge();
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
