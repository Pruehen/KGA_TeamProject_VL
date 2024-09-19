using EnumTypes;
using System;
using UnityEngine;
public class PlayerInstanteState : MonoBehaviour
{
    PlayerMaster _PlayerMaster;

    Combat combat;
    Combat shield;


    public float hp { get => combat.GetHp(); private set => combat.ForceChangeHp(value); }
    public float Shield { get => shield.GetHp(); private set => shield.ForceChangeHp(value); }
    public float stamina { get; private set; }
    public int bullets { get; private set; }
    public int meleeBullets { get; private set; }
    public float skillGauge { get; private set; }
    public float DefaultAttackSpeed { get => attackSpeed; private set => attackSpeed = value; }
    public float AttackSpeed()
    {
        int level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Generic3);
        if (level > 0)
        {
            float percentageIncrease;

            if (skillGauge >= 400)
            {
                percentageIncrease = JsonDataManager.GetBlueChipData(BlueChipID.Generic3).Level_VelueList[level][3] / 100f;
                return DefaultAttackSpeed * (1 + percentageIncrease);
            }
            else if (skillGauge >= 300)
            {
                percentageIncrease = JsonDataManager.GetBlueChipData(BlueChipID.Generic3).Level_VelueList[level][2] / 100f;
                return DefaultAttackSpeed * (1 + percentageIncrease);
            }
            else if (skillGauge >= 200)
            {
                percentageIncrease = JsonDataManager.GetBlueChipData(BlueChipID.Generic3).Level_VelueList[level][1] / 100f;
                return DefaultAttackSpeed * (1 + percentageIncrease);
            }
            else if (skillGauge >= 100)
            {
                percentageIncrease = JsonDataManager.GetBlueChipData(BlueChipID.Generic3).Level_VelueList[level][0] / 100f;
                return DefaultAttackSpeed * (1 + percentageIncrease);
            }
            else
            {
                return DefaultAttackSpeed;
            }
        }
        else
        {
            return DefaultAttackSpeed;
        }
    }
    public int MeleeToRangeRatio { get => _meleeToRangeRatio; private set => _meleeToRangeRatio = value; }
    public float AbsorbingStaminaConsumRate { get => _absorbingStaminaConsumRate; private set => _absorbingStaminaConsumRate = value; }

    [SerializeField] private float _absorbingStaminaConsumRate = 300f;
    [SerializeField] SkinnedMeshRenderer _PlayerMesh;
    [SerializeField] Material _ShieldMaterial;


    [SerializeField] private int _meleeToRangeRatio = 2;

    public Action<Combat> OnDead;

    bool _isMeleeMode;
    public bool IsMeleeMode
    {
        get => _PlayerMaster.PlayerAttack.IsMelee;
    }

    public bool IsDead { get => combat.IsDead(); }
    float _maxHpBase;
    [SerializeField]
    float maxHpBase
    {
        get => _maxHpBase;
        set
        {
            _maxHpBase = value;
            combat.SetMaxHp(GetMaxHp());
        }
    }
    float _maxHpMulti = 1f;
    public float MaxHpMulti
    {
        get => _maxHpMulti;
        set
        {
            _maxHpMulti = value;
            combat.SetMaxHp(GetMaxHp());
        }
    }
    public float GetMaxHp() { return maxHpBase * MaxHpMulti; }

    float _maxShieldBase;
    [SerializeField]
    float maxShieldBase
    {
        get => _maxShieldBase;
        set
        {
            _maxShieldBase = value;
            shield.SetMaxHp(GetMaxShield());
        }
    }
    float _maxShieldMulti = 1f;
    public float MaxShieldMulti
    {
        get => _maxShieldMulti;
        set
        {
            _maxShieldMulti = value;
            shield.SetMaxHp(GetMaxShield());
        }
    }
    public float GetMaxShield() { return maxShieldBase * MaxShieldMulti; }
    [SerializeField] float MaxStamina;
    [SerializeField] float staminaRecoverySpeed;
    [SerializeField] float staminaRecoveryDelay;
    float staminaRecoveryDelayValue = 99f;

    [SerializeField] float MaxskillGauge = 400f;
    float skillGaugeRecoveryRestTime = 0f;
    public float SkillGaugeRecoveryMulti { get; set; } = 1f;
    [SerializeField] int maxBullets = 50;
    [SerializeField] int maxMeleeBullets = 50;
    public void Passive_Utility5_Active(int value1, int value2) { maxBullets += value1; maxMeleeBullets += value2; }

    [SerializeField] float attackSpeed = 1f;
    [SerializeField] float attackPowerBase;
    public float AttackPowerMulti { get; set; } = 1f;
    public float GetAttackPower() { return attackPowerBase * AttackPowerMulti; }
    [SerializeField] Vector3 attackRangeBase = new Vector3(1f, 1f, 1f);
    public Vector3 attackRangeMulti { get; set; } = new Vector3(1f, 1f, 1f);
    public Vector3 GetAttackRange() { return Vector3.Scale(attackRangeBase, attackRangeMulti); }
    [SerializeField] float skillRangeBase = 1f;
    public float skillRangeMulti { get; set; } = 1f;
    public float GetSkillRange() { return skillRangeBase * skillRangeMulti; }
    [SerializeField] float skillPowerBase;
    public float SkillPowerMulti { get; set; } = 1f;
    public float GetSkillPower() { return skillPowerBase * SkillPowerMulti; }
    public float DmgMulti { get; set; } = 1f;

    public float ChargeTime { get; set; } = 1f;

    [SerializeField] public float _dashTime = .5f;
    [SerializeField] public float _dashForce = 3f;
    [SerializeField] public float _dashCost = 300f;

    public float DashTime
    {
        get { return _dashTime * (1f + DashTimeMulti); }
        set { _dashTime = value; }
    }
    public float DashForce
    {
        get { return _dashForce * (1f + DashForceMulti); }
        set { _dashForce = value; }
    }
    public float DashCost
    {
        get { return _dashCost * (1f + DashCostMulti); }
        set { _dashCost = value; }
    }

    public float DashTimeMulti = 0f;
    public float DashForceMulti = 0f;
    public float DashCostMulti = 0f;


    Passive_Offensive1 passive_Offensive1;
    Passive_Offensive2 passive_Offensive2;
    Passive_Offensive3 passive_Offensive3;
    Passive_Offensive4 passive_Offensive4;
    Passive_Offensive5 passive_Offensive5;

    Passive_Defensive1 passive_Defensive1;
    Passive_Defensive2 passive_Defensive2;
    Passive_Defensive3 passive_Defensive3;
    Passive_Defensive4 passive_Defensive4;
    Passive_Defensive5 passive_Defensive5;

    Passive_Utility1 passive_Utility1;
    Passive_Utility2 passive_Utility2;
    Passive_Utility3 passive_Utility3;
    Passive_Utility4 passive_Utility4;
    Passive_Utility5 passive_Utility5;

    [SerializeField] LayerMask LayerMask_EnemyCheck;
    void Passive_Offensive2_Active_OnUpdate()
    {
        if (passive_Offensive2 != null)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, passive_Offensive2.CheckDistance_ToEnemy, LayerMask_EnemyCheck);
            foreach (var item in colliders)
            {
                if (item.transform.parent != null && item.transform.parent.TryGetComponent(out EnemyBase enemy))
                {
                    enemy.ActiveDebuff_Passive_Offensive2(passive_Offensive2.DmgGain);
                }
            }
        }
    }

    [SerializeField] int executionCount = 0;//체력2 패시브에서 사용
    public int ExecutionCount
    {
        get { return executionCount; }
    }

    public Action OnDamaged;

    public void OnEnemyDestroy()
    {
        Passive_Defensive2_AddExcutionCount();
        Passive_Defensive4_Active();
    }

    void Passive_Defensive2_AddExcutionCount()
    {
        executionCount++;
        if (passive_Defensive2 != null && executionCount >= passive_Defensive2.CountCheck)
        {
            executionCount = 0;
            passive_Defensive2.Active();
        }
    }

    void Passive_Defensive4_Active()
    {
        if (passive_Defensive4 != null)
        {
            passive_Defensive4.Active();
        }
    }
    public void OnMeleeModeChange(bool isMelee)
    {
        if (passive_Defensive5 != null && isMelee == false)
        {
            passive_Defensive5.Active();
        }
    }

    float _holdTime_Passive_Defensive3 = 0;//체력3 패시브에서 사용

    public float GetDmg(PlayerAttackModule attack, bool isLastAttack = false)
    {
        float baseDmg = GetAttackPower() * GetDamageMultiByAttakcType(attack);// * coefficient;
        float dmgGain = DmgMulti;
        if (attack is PlayerMeleeAttack melee && melee.IsCharged)//차지 공격일 경우
        {
            int level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Melee1);
            if (level > 0)
            {
                baseDmg += ((hp + Shield) * JsonDataManager.GetBlueChipData(BlueChipID.Melee1).Level_VelueList[level][0]) * 0.01f;
            }
        }
        else//평타, 대시공격, 차지공격일 경우
        {
            if (_PlayerMaster._PlayerBuff.blueChip4_Buff_NextHitAddDmg.TryDequeue(out float addDmgGain))
            {
                dmgGain += addDmgGain;
                Debug.Log("피해증가 버프 소모");
            }
        }

        //"원거리 공격 시 탄환을 {0}만큼 추가로 소모. 원거리 타격 피해량 {1}% 증가. 근거리 공격 시 근접 게이지를 {2}만큼 추가로 소모. 근거리 타격 피해량 {3}% 증가",
        int blueChip7Level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Generic2);
        if (blueChip7Level > 0)
        {
            if (attack is PlayerMeleeAttack)//근접 공격일 경우
            {
                if (meleeBullets > 0)
                {
                    float addDmg = JsonDataManager.GetBlueChipData(BlueChipID.Generic2).Level_VelueList[blueChip7Level][1] * 0.01f;
                    dmgGain += addDmg;
                }
            }
            else//원거리 공격일 경우
            {
                if (bullets > 0)
                {
                    float addDmg = JsonDataManager.GetBlueChipData(BlueChipID.Generic2).Level_VelueList[blueChip7Level][1] * 0.01f;
                    dmgGain += addDmg;
                }
            }
        }
        //=========================================================================================================================================================

        float finalDmg = baseDmg * dmgGain;
        if (passive_Offensive5 != null)
        {
            finalDmg += passive_Offensive5.ValueChangeRatio * (GetSkillPower());
        }
        return finalDmg;
    }
    public Vector3 GetRange(PlayerAttackModule attack)
    {
        attackRangeBase = _PlayerMaster.SkillData.MeleedefaultAttackRange;

        float rangeGain = 1f;

        if (attack is PlayerMeleeAttack melee && melee.IsCharged)
        {
            attackRangeBase = _PlayerMaster.SkillData.MeleeChargedAttackRange;
            int level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Melee1);
            if (level > 0)
            {
                rangeGain = (JsonDataManager.GetBlueChipData(BlueChipID.Melee1).Level_VelueList[level][1]);
            }
            else
            {
                rangeGain = 1f;
            }
        }
        Vector3 baseRange = GetAttackRange();
        return baseRange * rangeGain;
    }

    public float GetSkillDmg()
    {
        //attackRangeMulti = _PlayerMaster.SkillData.MeleedefaultAttackRange;
        float baseDmg = GetSkillPower();// * coefficient;
        float dmgGain = 1;

        int level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Melee1);
        if (level > 0)
        {
            baseDmg += ((hp + Shield) * JsonDataManager.GetBlueChipData(BlueChipID.Melee1).Level_VelueList[level][0]) * 0.01f;
        }


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

    [SerializeField] public SO_Player _playerStatData;

    private void Start()
    {
        //UIManager.Instance.UpdateStamina(stamina, MaxStamina);

        UpdateHealth();
        UpdateStamina();
        UpdateSkillGauge();
    }

    private void Update()
    {
        combat.DoUpdate(Time.deltaTime);
        shield.DoUpdate(Time.deltaTime);
        TestSkill();

        Passive_Offensive2_Active_OnUpdate();

        if (_PlayerMaster.IsAbsorbing)
        {
            TryStaminaConsumption(_absorbingStaminaConsumRate * Time.deltaTime);
        }

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

        _holdTime_Passive_Defensive3 -= Time.deltaTime;
    }

    public void Init()
    {
        _PlayerMaster = GetComponent<PlayerMaster>();

        combat = new Combat();
        shield = new Combat();

        combat.Init(gameObject, GetMaxHp());

        combat.OnKnockback += OnKnockback;
        combat.OnDead += HandleOnDead;

        shield.Init(gameObject, GetMaxShield());
        shield.OnKnockback += OnKnockback;

        maxHpBase = _playerStatData.maxHp;
        MaxStamina = _playerStatData.MaxStamina;
        staminaRecoverySpeed = _playerStatData.staminaRecoverySpeed;
        staminaRecoveryDelay = _playerStatData.staminaRecoveryDelay;

        MaxskillGauge = _playerStatData.MaxskillGauge;
        maxBullets = _playerStatData.maxBullets;
        maxMeleeBullets = _playerStatData.maxMeleeBullets;

        attackSpeed = _playerStatData.attackSpeed;
        attackPowerBase = _playerStatData.attackPower;
        skillPowerBase = _playerStatData.skillPower;
        ChargeTime = _playerStatData.chargeTime;

        moveSpeed = _playerStatData.moveSpeed;

        maxShieldBase = _playerStatData.shieldMax;

        DashTime = _playerStatData.dashTime;
        DashForce = _playerStatData.dashForce;
        DashCost = _playerStatData.dashCost;


        _PlayerMaster.Mod.OnModChanged += OnMeleeModeChange;


        InitPassive();
        Restore();
    }

    public void Init_OnSceneLoad()
    {
        UserData userData = JsonDataManager.GetUserData();
        if (userData.TryGetPlayData(out PlayData playData))
        {
            if (playData.InGame_Hp == -1f)
            {
                Restore();
            }
            else
            {
                combat.ForceChangeHp(playData.InGame_Hp);

                skillGauge = playData.InGame_SkillGauge;
                bullets = playData.InGame_Bullet;
                meleeBullets = playData.InGame_MeleeBullet;
                _PlayerMaster.Mod.ChangeModOnly(playData.InGame_IsMelee);
            }
        }
        userData.InitPlayData(userData.Gold);
        UpdateBullet();
        UpdateBullet_Melee();
    }

    //스태미나 소모 
    public bool TryStaminaConsumption(float power)
    {
        if (stamina <= 0)
        {
            return false;
        }
        else
        {
            stamina -= power;
            if (stamina <= 0)
            {
                stamina = 0;
                staminaRecoveryDelayValue = 0;
            }
            UpdateStamina();
            return true;
        }
    }

    public void StaminaRatioChange(float value)
    {
        stamina += MaxStamina * value;
        if (stamina > MaxStamina)
        {
            stamina = MaxStamina;
        }

        UpdateStamina();
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

    public void Hit(float dmg, out float finalDmg, DamageType damageType = DamageType.Normal)
    {
        if (combat.IsInvincible || shield.IsInvincible)
        {
            finalDmg = 0;
            return;
        }

        OnDamaged?.Invoke();

        if (passive_Defensive4 != null)
        {
            dmg -= dmg * passive_Defensive4.Value_DamageReductionPercentage * 0.01f;
            passive_Defensive4.DeActive();
        }

        finalDmg = dmg;

        if (Shield > 0)
        {
            float s = shield.GetHp();
            s -= dmg;
            shield.Damaged(dmg, damageType);
            if (s < 0)
            {
                dmg = -s;
            }
            UpdateShild();
            return;
        }
        float tempHp = hp;
        tempHp -= dmg;
        if (tempHp <= 0)
        {
            if (passive_Defensive3 != null && passive_Defensive3.ActiveCount > 0)
            {
                passive_Defensive3.Active(out _holdTime_Passive_Defensive3);

                combat.SetInvincible(_holdTime_Passive_Defensive3);
                combat.ForceChangeHp(passive_Defensive3.HpHoldValue);
                finalDmg = 0;

                Debug.Log("무적 발동!");

            }
            if (combat.IsInvincible)
            {
                finalDmg = 0;
            }
            else
            {
                combat.Damaged(dmg, damageType);
            }
            //if (_holdTime_Passive_Defensive3 > 0)
            //{
            //    hp = passive_Defensive3.HpHoldValue;
            //    Debug.Log("핫하 무적이다!");
            //    finalDmg = 0;
            //}
        }
        else
        {
            combat.Damaged(dmg, damageType);
        }
        UpdateHealth();
    }
    void HandleOnDead(Combat self)
    {
        Debug.Log("플레이어 사망");
        OnDead?.Invoke(self);

        GameManager.Instance.OnPlayerDead();

        Destroy(this.gameObject);
    }

    public void ChangeHp(float value)
    {
        combat.SetMaxHp(GetMaxHp());

        combat.ForceChangeHp(value);

        UpdateHealth();
    }

    public void ChangeShield(float value)
    {
        shield.ForceChangeHp(value);
        UpdateShild();
    }

    //탄환 획득
    public void AcquireBullets(int _bullets)
    {
        bullets += _bullets;
        SM.Instance.PlaySound2("getBullet", transform.position);
        if (bullets > maxBullets)
        {
            bullets = maxBullets;
        }
        UpdateBullet();
    }

    //탄환 소모
    public void BulletConsumption()
    {
        if (_PlayerMaster.GetBlueChipLevel(EnumTypes.BlueChipID.Hybrid2) > 0)
        {
            return;
        }

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
        meleeBullets += _bullets / MeleeToRangeRatio;
        SM.Instance.PlaySound2("getBullet", transform.position);
        if (meleeBullets > maxBullets)
        {
            meleeBullets = maxBullets;
        }
        UpdateBullet_Melee();
    }

    //근접탄 소모
    public void BulletConsumption_Melee()
    {
        if (_PlayerMaster.GetBlueChipLevel(EnumTypes.BlueChipID.Hybrid2) > 0)
        {
            return;
        }
        int blueChip7Level = _PlayerMaster.GetBlueChipLevel(BlueChipID.Generic2);
        int cost = (blueChip7Level > 0) ? (int)JsonDataManager.GetBlueChipData(BlueChipID.Generic2).Level_VelueList[blueChip7Level][2] : 1;

        meleeBullets -= cost;
        SM.Instance.PlaySound2("playerMeleeAttackHit", transform.position);
        Debug.Log("근접탄소모");
        if (meleeBullets < 0)
            meleeBullets = 0;
        UpdateBullet_Melee();
    }

    public void BulletClear_Melee()
    {
        meleeBullets = 0;
        UpdateBullet_Melee();
    }

    private float GetSkillGainOnHit(PlayerAttackModule currentAttack)
    {
        if (currentAttack is PlayerRangeAttack range)
        {
            if (range.IsDashAttack) return _playerStatData.statGaugeGainRanged3;
            if (range.IsLastAttack) return _playerStatData.statGaugeGainRanged2;
            return _playerStatData.statGaugeGainRanged1;
        }
        else if (currentAttack is PlayerMeleeAttack melee)
        {
            if (melee.IsDashAttack) return _playerStatData.statGaugeGainMelee3;
            if (melee.IsCharged) return _playerStatData.statGaugeGainMelee2;
            return _playerStatData.statGaugeGainMelee1;
        }
        return 0f;
    }
    private float GetSkillGainOnHit(bool isMelee, bool isCharge, bool isDash, bool isLastAttack)
    {

        if (!isMelee)
        {
            if (isDash) return _playerStatData.statGaugeGainRanged3;
            if (isLastAttack) return _playerStatData.statGaugeGainRanged2;
            return _playerStatData.statGaugeGainRanged1;
        }
        else if (isMelee)
        {
            if (isDash) return _playerStatData.statGaugeGainMelee3;
            if (isCharge) return _playerStatData.statGaugeGainMelee2;
            return _playerStatData.statGaugeGainMelee1;
        }
        return 0f;
    }
    private float GetDamageMultiByAttakcType(PlayerAttackModule attack)
    {
        bool enhanced = bullets > 0;

        if (attack is PlayerMeleeAttack melee)
        {

            if (melee.IsDashAttack) return _playerStatData.atkMelee121;
            if (melee.IsCharged) return _playerStatData.atkMelee111;
            return _playerStatData.atkMelee101;
        }
        if (attack is PlayerRangeAttack range)
        {
            if (enhanced)
            {
                if (range.IsDashAttack) return _playerStatData.atkRanged113;
                if (range.IsLastAttack) return _playerStatData.atkRanged112;
                return _playerStatData.atkRanged111;
            }
            else
            {
                if (range.IsDashAttack) return _playerStatData.atkRanged103;
                if (range.IsLastAttack) return _playerStatData.atkRanged102;
                return _playerStatData.atkRanged101;
            }
        }


        return 1f;
    }
    public void SkillGaugeRecovery(float value)
    {
        skillGauge += value * SkillGaugeRecoveryMulti;

        if (skillGauge > MaxskillGauge)
        {
            skillGauge = MaxskillGauge;
        }

        skillGaugeRecoveryRestTime = 0;
        UpdateSkillGauge();
        Debug.Log($"SkillGauge recover {value}");
    }
    public void SkillGaugeRecovery(PlayerAttackModule attack, int count = 1)
    {
        SkillGaugeRecovery(GetSkillGainOnHit(attack) * count);
    }
    public void SkillGaugeRecovery(bool isDash, bool isLastAttack)
    {
        SkillGaugeRecovery(GetSkillGainOnHit(false, false, isDash, isLastAttack));
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

    public void Restore()
    {
        hp = GetMaxHp();
        combat.ResetCombat();
        stamina = MaxStamina;
        skillGauge = 0;
        bullets = maxBullets / 3;
        DefaultAttackSpeed = 1;

        Shield = 0f;
        UpdateHealth();
        UpdateShild();
        UpdateBullet();
        UpdateBullet_Melee();
        UpdateSkillGauge();
        UpdateStamina();
    }
    void InitPassive()
    {
        PlayerPassive playerPassive = _PlayerMaster._PlayerPassive;
        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        if (playerPassive.ContainPassiveId(PassiveID.Offensive1))
        {
            passive_Offensive1 = new Passive_Offensive1();
            passive_Offensive1.Init(this);
        }
        if (playerPassive.ContainPassiveId(PassiveID.Offensive2))
        {
            passive_Offensive2 = new Passive_Offensive2();
            passive_Offensive2.Init(this);
        }
        if (playerPassive.ContainPassiveId(PassiveID.Offensive3))
        {
            passive_Offensive3 = new Passive_Offensive3();
            passive_Offensive3.Init(this);
            passive_Offensive3.Active();
        }
        if (playerPassive.ContainPassiveId(PassiveID.Offensive4))
        {
            passive_Offensive4 = new Passive_Offensive4();
            passive_Offensive4.Init(this);
            passive_Offensive4.Active();
        }
        if (playerPassive.ContainPassiveId(PassiveID.Offensive5))
        {
            passive_Offensive5 = new Passive_Offensive5();
            passive_Offensive5.Init(this);
        }
        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        if (playerPassive.ContainPassiveId(PassiveID.Defensive1))
        {
            passive_Defensive1 = new Passive_Defensive1();
            passive_Defensive1.Init(this);
            passive_Defensive1.Active();
        }
        if (playerPassive.ContainPassiveId(PassiveID.Defensive2))
        {
            passive_Defensive2 = new Passive_Defensive2();
            passive_Defensive2.Init(this);
        }
        if (playerPassive.ContainPassiveId(PassiveID.Defensive3))
        {
            passive_Defensive3 = new Passive_Defensive3();
            passive_Defensive3.Init(this);
        }
        if (playerPassive.ContainPassiveId(PassiveID.Defensive4))
        {
            passive_Defensive4 = new Passive_Defensive4();
            passive_Defensive4.Init(this);
        }
        if (playerPassive.ContainPassiveId(PassiveID.Defensive5))
        {
            passive_Defensive5 = new Passive_Defensive5();
            passive_Defensive5.Init(this);
        }
        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
        if (playerPassive.ContainPassiveId(PassiveID.Utility1))
        {
            passive_Utility1 = new Passive_Utility1();
            passive_Utility1.Init(this);
        }
        if (playerPassive.ContainPassiveId(PassiveID.Utility2))
        {
            passive_Utility2 = new Passive_Utility2();
            passive_Utility2.Init(this);
            passive_Utility2.Active();
        }
        if (playerPassive.ContainPassiveId(PassiveID.Utility3))
        {
            passive_Utility3 = new Passive_Utility3();
            passive_Utility3.Init(this);
        }
        if (playerPassive.ContainPassiveId(PassiveID.Utility4))
        {
            passive_Utility4 = new Passive_Utility4();
            passive_Utility4.Init(this);
        }
        if (playerPassive.ContainPassiveId(PassiveID.Utility5))
        {
            passive_Utility5 = new Passive_Utility5();
            passive_Utility5.Init(this);
            passive_Utility5.Active();
        }
        //=======================================================================================
        //=======================================================================================
        //=======================================================================================
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
        float hpRatio = combat.GetHpRatio();
        HealthRatioChanged?.Invoke(hpRatio);

        if (passive_Offensive1 != null)
        {
            if (passive_Offensive1.HpCheckRetio <= hpRatio)
            {
                passive_Offensive1.Active();
            }
            else
            {
                passive_Offensive1.DeActive();
            }
        }
    }
    public void UpdateShild()
    {
        ShieldMT();
        ShildRatioChanged?.Invoke(shield.GetHpRatio());
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
    public void TestSkill()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SkillGaugeRecovery(100f);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SkillGaugeRecovery(200f);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SkillGaugeRecovery(300f);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SkillGaugeRecovery(400f);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            combat.Heal(999f);
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            GameManager.Instance.KillAll();
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            GameManager.Instance.TPToDoor();
        }
    }

    //골드 관련

    private float _goldMulti = 1f;
    public float GoldMulti { get { return _goldMulti; } set { _goldMulti = value; } }
    public void AddGold(int amount)
    {
        if (JsonDataManager.GetUserData().TryGetPlayData(out PlayData playData))
        {
            amount = (int)(amount * GoldMulti);
            playData.AddGold_InGame(amount);
            UIManager.Instance.UpdateGoldInfoUI();
        }
    }

    private void OnKnockback()
    {
        _PlayerMaster.OnKnockback();
    }

    public void SetInvincible(float dashTime)
    {
        combat.SetInvincible(dashTime);
        shield.SetInvincible(dashTime);
    }

    public void SetSuperArmor(float time)
    {
        combat.SetSuperArmor(time);
        shield.SetSuperArmor(time);
    }

    public void ResetSuperArmor()
    {
        combat.ResetSuperArmor();
        shield.ResetSuperArmor();
    }

    public void SetEvade(float v)
    {
        combat.SetEvade(v);
        shield.SetEvade(v);
    }

    public void ResetEvade()
    {
        combat.ResetEvade();
        shield.ResetEvade();
    }

    internal void ResetInvincible()
    {
        combat.ResetInvincible();
        shield.ResetInvincible();
    }


    public void ShieldMT()
    {
        //Debug.Log(Shield);

        // 현재 materials 배열을 가져옵니다.
        Material[] materials = _PlayerMesh.materials;

        if (Shield > 0)
        {
            materials[1] = _ShieldMaterial;
            Debug.Log("실드있음");
        }
        else
        {
            materials[1] = null;  // 여기서는 재질을 null로 설정할 수 없으므로, 제거해야 합니다.
            Debug.Log("실드없음");
        }

        // 수정된 배열을 다시 SkinnedMeshRenderer에 할당합니다.
        _PlayerMesh.materials = materials;
    }

    public void HealShield(float v)
    {
        shield.ResetDead();
        shield.Heal(v);
        UpdateShild();
    }

    public void SetMaxShield(float v)
    {
        shield.SetMaxHp(v);
        UpdateShild();
    }

    public void Passive_Utility4_Active()
    {
        if (passive_Utility4 != null)
        {
            passive_Utility4.Active();
        }
    }
}
