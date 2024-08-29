using EnumTypes;
using System.ComponentModel;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject Prefab_Projectile;

    [SerializeField] Transform projectile_InitPos;
    [SerializeField] float projectionSpeed_Forward = 15;
    [SerializeField] float projectionSpeed_Up = 3;
    [SerializeField] float attack_CoolTime = 0.7f;

    [SerializeField] SO_SKillEvent EnterMelee;
    [SerializeField] SO_SKillEvent EnterRangeR;
    [SerializeField] SO_SKillEvent EnterRangeL;
    InputManager _InputManager;
    PlayerCameraMove _PlayerCameraMove;
    PlayerMaster _PlayerMaster;
    AttackSystem _AttackSystem;
    PlayerModChangeManager _PlayerMod;
    Animator _animator;

    float delayTime = 0;
    public bool attackTrigger = false;
    bool attackBool = false;

    bool skillTrigger = false;
    bool skillBool = false;

    [SerializeField] PlayerAttackKind _currentAttackMod = PlayerAttackKind.RangeNormalAttack;
    [SerializeField]
    PlayerAttackKind CurrentAttackKind
    {
        get
        {
            if (IsDashAttack() && !IsLastAttack())
            {
                return (_currentAttackMod == PlayerAttackKind.RangeNormalAttack) ? PlayerAttackKind.RangeDashAttack : PlayerAttackKind.MeleeDashAttack;
            }
            return _currentAttackKind;
        }
        set
        {
            _currentAttackKind = value;
        }
    }
    [SerializeField] PlayerAttackKind _currentAttackKind = PlayerAttackKind.RangeNormalAttack;
    [SerializeField] int _initialAttackComboIndex = 0;
    int _currentAttackCount;

    public float attack_ChargeTime { get => _AttackSystem.CloseAttack.ChargeTime; set => _AttackSystem.CloseAttack.ChargeTime = value; }


    public void Init()
    {
        _InputManager = InputManager.Instance;
        _InputManager.PropertyChanged += OnInputPropertyChanged;

        _PlayerCameraMove = PlayerCameraMove.Instance;

        _PlayerMaster = GetComponent<PlayerMaster>();
        _AttackSystem = GetComponent<AttackSystem>();
        _PlayerMod = GetComponent<PlayerModChangeManager>();

        _PlayerMod.OnSucceseAbsorpt += ChangeAttackState;
        _PlayerMod.OnEnterAbsorptState += ChangeAbsorbing;
        _PlayerMod.OnEndAbsorptState += AbsorbingFall;

        _AttackSystem.Init(Callback_IsCharged, Callback_IsChargedFail, Callback_IsChargedEnd, Callback_IsChargedStart);

        _animator = GetComponent<Animator>();
        _PlayerMaster._PlayerInstanteState.OnMeleeModeChanged += OnModChanged;

    }

    private void OnDestroy()
    {
        _PlayerMod.OnSucceseAbsorpt -= ChangeAttackState;
        _InputManager.PropertyChanged -= OnInputPropertyChanged;
        _PlayerMod.OnEnterAbsorptState -= AbsorbingFall;
    }

    private void ChangeAbsorbing()
    {
        _AttackSystem.Absober();
    }
    private void AbsorbingFall()
    {
        Debug.Log("AbsorbingFall");
        //ChangeAttackState(false);
        _AttackSystem.AbsoberEnd();
        ObjectPoolManager.Instance.AllDestroyObject(_AttackSystem.startAbsorbing.preFab);
    }
    private void ChangeAttackState(bool isMelee)
    {
        if (isMelee)
        {
            CurrentAttackKind = PlayerAttackKind.MeleeNormalAttack;
            _currentAttackMod = PlayerAttackKind.MeleeNormalAttack;
            _AttackSystem.ModTransform();
            EnterMeleeVFX();
        }
        else
        {
            if(_currentAttackMod!= PlayerAttackKind.RangeNormalAttack)
            {
                EnterRangeVFX();
            }
            CurrentAttackKind = PlayerAttackKind.RangeNormalAttack;
            _currentAttackMod = PlayerAttackKind.RangeNormalAttack;
            //_AttackSystem.ModTransform();
        }
    }

    public void EnterRangeVFX()
    {
        _PlayerMaster._PlayerSkill.Effect2(EnterRangeL);
        _PlayerMaster._PlayerSkill.Effect2(EnterRangeR);
    }
    public void EnterMeleeVFX()
    {
        _PlayerMaster._PlayerSkill.Effect2(EnterMelee);
    }
    private void OnModChanged(bool isMelee)
    {
        if ((int)_currentAttackMod == (isMelee ? 1 : 0))
        {
            if (!isMelee)
            {
                CurrentAttackKind = PlayerAttackKind.RangeNormalAttack;
                _currentAttackMod = PlayerAttackKind.RangeNormalAttack;
            }
            else
            {
                CurrentAttackKind = PlayerAttackKind.MeleeNormalAttack;
                _currentAttackMod = PlayerAttackKind.MeleeNormalAttack;
            }
        }
    }

    bool prevAttackTrigger = false;


    int initialAttackComboIndex;
    [Header("total count of attack animation")]
    [SerializeField] int _totalAttackAnimCount = 4;
    private void Update()
    {
        delayTime += Time.deltaTime;
        //if(attackTrigger && delayTime >= attack_CoolTime + attack_Delay && !_PlayerMaster.IsMeleeMode && !_PlayerMaster.IsAbsorptState)
        if (attackTrigger && !prevAttackTrigger)
        {
            delayTime = 0;

            int blueChip2Level = _PlayerMaster.GetBlueChipLevel(EnumTypes.BlueChipID.Range1);
            initialAttackComboIndex = (blueChip2Level > 0) ? (int)JsonDataManager.GetBlueChipData(EnumTypes.BlueChipID.Range1).Level_VelueList[blueChip2Level][0] : 0;
            _AttackSystem.StartAttack(_currentAttackMod, _currentAttackKind, initialAttackComboIndex);
            //StartCoroutine(Attack_Delayed(attack_Delay));
        }
        if (!attackTrigger && prevAttackTrigger)
        {
            if (_currentAttackMod == PlayerAttackKind.MeleeNormalAttack)
            {
                _AttackSystem.OnRelease();
            }
        }
        //if (skillTrigger && !prevAttackTrigger)
        if (skillTrigger && skillBool)
        {
            skillBool = false;
            delayTime = 0;
            _AttackSystem.StartSkill((int)CurrentAttackKind, _PlayerMaster._PlayerInstanteState.skillGauge);
        }
        prevAttackTrigger = attackTrigger;
    }
    public int GetCurrentAttackCount()
    {
        return _currentAttackCount;
    }
    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_InputManager.IsLMouseBtnClick):
                if (!_PlayerMaster._PlayerInstanteState.IsAbsorptState)
                {
                    attackTrigger = _InputManager.IsLMouseBtnClick;
                    attackBool = _InputManager.IsLMouseBtnClick;
                }
                break;
            case nameof(_InputManager.IsRMouseBtnClick):
                if (!_PlayerMaster._PlayerInstanteState.IsAbsorptState)
                {
                    skillTrigger = _InputManager.IsRMouseBtnClick;
                    skillBool = _InputManager.IsRMouseBtnClick;
                }
                break;
        }
    }

    void ShootProjectile()
    {
        if (_animator.IsInTransition(0))
        {
            return;
        }

        IncreaseAttackCount();
        Projectile projectile = ObjectPoolManager.Instance.DequeueObject(Prefab_Projectile).GetComponent<Projectile>();

        Vector3 projectionVector = transform.forward * projectionSpeed_Forward + Vector3.up * projectionSpeed_Up;

        projectile.Init(_PlayerMaster._PlayerInstanteState.GetDmg(CurrentAttackKind,
            IsLastAttack()),
            projectile_InitPos.position, projectionVector,
            _currentAttackMod, CurrentAttackKind, _currentAttackCount,
            OnRangeHit);

        _PlayerMaster._PlayerInstanteState.BulletConsumption();

        int level_blueChip_Range2 = _PlayerMaster.GetBlueChipLevel(BlueChipID.Range2);
        if (IsLastAttack() && level_blueChip_Range2 > 0)//"원거리 마지막 공격 시, {0}%의 확률로 {1}% 위력의 무작위 스킬 발동",
        {
            BlueChip chip_Range2 = JsonDataManager.GetBlueChipData(BlueChipID.Range2);

            float skillActivationProbability = chip_Range2.Level_VelueList[level_blueChip_Range2][0] * 0.01f;
            float skillActivationProbabilityValue = Random.Range(0f, 1f);

            if (skillActivationProbabilityValue < skillActivationProbability)
            {
                PlayerSkill randomSkill1 = (PlayerSkill)chip_Range2.Level_VelueList[level_blueChip_Range2][2];
                PlayerSkill randomSkill2 = (PlayerSkill)chip_Range2.Level_VelueList[level_blueChip_Range2][3];
                float skillSelectionValue = Random.Range(0, 2);
                if (skillSelectionValue == 0)
                {
                    _PlayerMaster._PlayerSkill.InvokeSkillDamage(randomSkill1);
                    _PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._PlayerSkill.RangeSkill1);
                }
                else
                {
                    _PlayerMaster._PlayerSkill.InvokeSkillDamage(randomSkill2);
                    _PlayerMaster._PlayerSkill.Effect2(_PlayerMaster._PlayerSkill.RangeSkill2);
                }
            }
        }
    }
    private void EnableDamageBox_Player()
    {
        _AttackSystem.EnableDamageBox(
            _PlayerMaster._PlayerInstanteState.GetDmg(CurrentAttackKind),
            _PlayerMaster._PlayerInstanteState.GetRange(CurrentAttackKind, GetCurrentAttackCount()), OnMeleeHit);
    }

    public void ResetAttack()
    {
        _currentAttackCount = 0;
        _AttackSystem.ResetAttack();
    }
    public void IncreaseAttackCount()
    {
        _currentAttackCount++;
    }

    private void Callback_IsCharged()
    {
        if (_currentAttackMod == PlayerAttackKind.MeleeNormalAttack)
        {
            CurrentAttackKind = PlayerAttackKind.MeleeChargedAttack;

            Debug.Log("차-지 완료");
            Debug.Log(CurrentAttackKind);
              _animator.SetInteger("AttackType", (int)CurrentAttackKind);
        }
    }
    private void Callback_IsChargedEnd()
    {
        Debug.Log("차-지 끝");
        //if (_currentAttackMod == PlayerAttackKind.MeleeNormalAttack)
        //{
        //    CurrentAttackKind = PlayerAttackKind.MeleeNormalAttack;
        //}
    }

    private void Callback_IsChargedFail()
    {
        //if (_currentAttackMod == PlayerAttackKind.MeleeNormalAttack)
        //{
        //    CurrentAttackKind = PlayerAttackKind.MeleeNormalAttack;
            Debug.Log("차-지 실패");
        //}
    }
    private void Callback_IsChargedStart()
    {
        CurrentAttackKind = PlayerAttackKind.MeleeNormalAttack;
        _currentAttackMod = PlayerAttackKind.MeleeNormalAttack;
        _animator.SetInteger("AttackType", (int)CurrentAttackKind);
        Debug.Log("차-지 시작");
    }
    public void OnUseSkillGauge()
    {
        _PlayerMaster._PlayerInstanteState.TryUseSkillGauge2();
        skillBool = false;
    }

    private void OnMeleeHit()
    {
        PlayerInstanteState stat = _PlayerMaster._PlayerInstanteState;
        stat.SkillGaugeRecovery(_currentAttackMod, CurrentAttackKind, false);
        _PlayerMaster.OnMeleeHit();
    }
    private void OnRangeHit(PlayerAttackKind mod, PlayerAttackKind kind, int currentAttackCount)
    {
        PlayerInstanteState stat = _PlayerMaster._PlayerInstanteState;
        stat.SkillGaugeRecovery(mod, kind, IsLastAttack(currentAttackCount));
    }

    private bool IsLastAttack()
    {
        return IsLastAttack(_currentAttackCount);
    }
    private bool IsLastAttack(int currentAttackCount)
    {
        if (currentAttackCount == 0)
            return false;
        if ((currentAttackCount) % (_totalAttackAnimCount - initialAttackComboIndex) == 0)
        {
            Debug.Log("막타");
            return true;
        }
        return false;
    }

    private bool IsDashAttack()
    {
        bool ran = AnimatorHelper.IsAnimationPlaying(_animator, 0, "Base Layer.Dash Attack");
        bool mel = AnimatorHelper.IsAnimationPlaying(_animator, 0, "Base Layer.Dash Range");
        return ran || mel;
    }

    private void SetSuperArmorOnAnim()
    {
        _PlayerMaster._PlayerInstanteState.SetSuperArmor(99999999f);
    }
    private void ResetSuperArmorOnAnim()
    {
        _PlayerMaster._PlayerInstanteState.ResetSuperArmor();
    }

    //회피는 대시시 스크립트에서 켜주기도 함
    private void SetEvadeOnAnim()
    {
        _PlayerMaster._PlayerInstanteState.SetEvade(99999999f);
    }
    private void ResetEvadeOnAnim()
    {
        _PlayerMaster._PlayerInstanteState.ResetEvade();
    }
    //IEnumerator Attack_Delayed(float delayTime)
    //{
    //    yield return new WaitForSeconds(delayTime);
    //    Attack();
    //}
    public void BlueChipFire()
    {
        //_PlayerMaster._PlayerEquipBlueChip.
            int level_blueChip_Generic1 = _PlayerMaster.GetBlueChipLevel(BlueChipID.Generic1);
        if (level_blueChip_Generic1 > 0)//블루칩이 1레벨 이상일경우
        {
            BlueChip chip_Generic1 = JsonDataManager.GetBlueChipData(BlueChipID.Generic1);
            float Dmg = 10+ level_blueChip_Generic1 * 2f;
            float fireDotDuration = 5f;
            float fireExploRadius = 1+(level_blueChip_Generic1 * 0.2f);
            float fireExploDmg = 50 + level_blueChip_Generic1 * 10f;
            GameObject _DashFire = ObjectPoolManager.Instance.DequeueObject(DashFire);
            DashFire DF = _DashFire.GetComponent<DashFire>();
            DF.transform.position=transform.position;
            DF.SetStat(Dmg, fireDotDuration, fireExploRadius, fireExploDmg);
            DF.StartFire();
        }
    }
    public GameObject DashFire;
}
