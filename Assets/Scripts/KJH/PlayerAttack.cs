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

    private void Awake()
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

        _AttackSystem.Init(Callback_IsCharged, Callback_IsChargedFail, Callback_IsChargedEnd);

        _animator = GetComponent<Animator>();
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
        ChangeAttackState(false);
        _AttackSystem.AbsoberEnd();
    }
    private void ChangeAttackState(bool isMelee)
    {
        if (isMelee)
        {
            CurrentAttackKind = PlayerAttackKind.MeleeNormalAttack;
            _currentAttackMod = PlayerAttackKind.MeleeNormalAttack;
            _AttackSystem.ModTransform();
        }
        else
        {
            CurrentAttackKind = PlayerAttackKind.RangeNormalAttack;
            _currentAttackMod = PlayerAttackKind.RangeNormalAttack;
            _AttackSystem.ModTransform();
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
            _PlayerMaster.OnAttackState(_PlayerCameraMove.CamRotation() * Vector3.forward);

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
            _PlayerMaster.OnAttackState(_PlayerCameraMove.CamRotation() * Vector3.forward);
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
        IncreaseAttackCount();
        Projectile projectile = ObjectPoolManager.Instance.DequeueObject(Prefab_Projectile).GetComponent<Projectile>();

        Vector3 projectionVector = _PlayerCameraMove.CamRotation() * Vector3.forward * projectionSpeed_Forward + Vector3.up * projectionSpeed_Up;
        //?�택?�스?�에???�재 공격???�?�을 가?�온??
        projectile.Init(_PlayerMaster._PlayerInstanteState.GetDmg(CurrentAttackKind,
            IsLastAttack()),
            projectile_InitPos.position, projectionVector,
            _currentAttackMod, CurrentAttackKind, _currentAttackCount,
            OnRangeHit);

        _PlayerMaster._PlayerInstanteState.BulletConsumption();
    }
    private void EnableDamageBox_Player()
    {
        _AttackSystem.EnableDamageBox(
            _PlayerMaster._PlayerInstanteState.GetDmg(CurrentAttackKind),
            _PlayerMaster._PlayerInstanteState.GetRange(CurrentAttackKind, GetCurrentAttackCount()),
OnMeleeHit
            );
    }

    public void ResetAttack()
    {
        _currentAttackCount = 0;
        //_AttackSystem.ResetAttack();
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
            Debug.Log("��-�� �Ϸ�");
        }
    }
    private void Callback_IsChargedEnd()
    {
        Debug.Log("��-�� ��");
    }

    private void Callback_IsChargedFail()
    {
        if (_currentAttackMod == PlayerAttackKind.MeleeNormalAttack)
        {
            CurrentAttackKind = PlayerAttackKind.MeleeNormalAttack;
            Debug.Log("��-�� ����");
        }
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
    private void OnRangeHit(PlayerAttackKind mod, PlayerAttackKind kind,int currentAttackCount)
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
        if(currentAttackCount == 0)
            return false;
        if ((currentAttackCount) % (_totalAttackAnimCount - initialAttackComboIndex) == 0)
        {
            Debug.Log("��Ÿ");
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
    //IEnumerator Attack_Delayed(float delayTime)
    //{
    //    yield return new WaitForSeconds(delayTime);
    //    Attack();
    //}
}
