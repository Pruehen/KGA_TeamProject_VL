using EnumTypes;
using System;
using System.ComponentModel;
using UnityEngine;


public class PlayerAttackSystem : MonoBehaviour
{
    InputManager _InputManager;
    PlayerCameraMove _PlayerCameraMove;
    PlayerMaster _PlayerMaster;
    public PlayerModChangeManager PlayerMod;

    private bool attackPressed = false;

    bool skillTrigger = false;

    [SerializeField] public PlayerAttackModule _currentAttack;
    [SerializeField] public PlayerMeleeAttack _meleeAttack;
    public PlayerRangeAttack _rangeAttack;
    Skill _closeSkill;

    [SerializeField] public SO_SKillEvent startAbsorbing;
    [SerializeField] public SO_SKillEvent endAbsorbing;
    [SerializeField] DamageBox _damageBox;

    private Animator _animator;


    [SerializeField] SO_SKillEvent EnterMelee;
    [SerializeField] SO_SKillEvent EnterRangeR;
    [SerializeField] SO_SKillEvent EnterRangeL;

    bool prevAttackPressed = false;

    public bool IsAnimAttack => AnimatorHelper.IsAnimCur_Tag(_animator, 0, "Attack");
    public bool IsCharging => _meleeAttack.IsCharging;
    public bool IsAttacking => IsAnimAttack;

    public bool IsMelee => _currentAttack == _meleeAttack;

    public void Init(Action onCharged = null, Action onChargeFail = null, Action onChargeEnd = null, Action onChargeStart = null)
    {
        _InputManager = InputManager.Instance;
        _InputManager.PropertyChanged += OnInputPropertyChanged;

        TryGetComponent(out _animator);
        TryGetComponent(out _closeSkill);

        _rangeAttack.Init(transform);

        _meleeAttack.Init(transform);
        _closeSkill.Init(_animator);
        _PlayerMaster = GetComponent<PlayerMaster>();


        PlayerMod.OnModChanged += ChangeAttackState;
        PlayerMod.OnModChangedVfx += ActiveMeleeTransformEffectAndAnim;
        PlayerMod.OnModChangedVfx += ActiveRangeVfx;

        PlayerMod.OnEnterAbsorb += StartAbsorbingAnimAndVsfx;
        PlayerMod.OnEndAbsorptState += DeactiveAbsortVfxAndAnim;
        PlayerMod.OnResetMod += ResetMod;
        
        PlayerMod.Init(transform);

        _currentAttack = _rangeAttack;

    }
    private void OnDestroy()
    {
        _InputManager.PropertyChanged -= OnInputPropertyChanged;
        PlayerMod.OnDestroy();

        
        PlayerMod.OnModChanged -= ChangeAttackState;
        PlayerMod.OnModChangedVfx -= ActiveMeleeTransformEffectAndAnim;
        PlayerMod.OnModChangedVfx -= ActiveRangeVfx;

        PlayerMod.OnEnterAbsorb -= StartAbsorbingAnimAndVsfx;
        PlayerMod.OnEndAbsorptState -= DeactiveAbsortVfxAndAnim;
        PlayerMod.OnResetMod -= ResetMod;
    }
    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(_InputManager.IsLMouseBtnClick):
                if (!_PlayerMaster.IsAbsorbing)
                {
                    attackPressed = _InputManager.IsLMouseBtnClick;
                    _animator.SetBool("IsAttackPressed", _InputManager.IsLMouseBtnClick);
                }
                break;
            case nameof(_InputManager.IsRMouseBtnClick):
                if (!_PlayerMaster.IsAbsorbing)
                {
                    skillTrigger = _InputManager.IsRMouseBtnClick;
                }
                break;
        }
    }
    public void ChangeAttackState(bool isMelee)
    {
        if (isMelee)
        {
            _currentAttack = _meleeAttack;
        }
        else
        {
            _currentAttack = _rangeAttack;

        }
        _animator.SetBool("IsMelee", isMelee);
    }
    public void ActiveMeleeTransformEffectAndAnim(bool isMelee)
    {
        if (!isMelee)
            return;
        _animator.SetTrigger("Transform");
        EnterMeleeVFX();
    }
    public void ActiveRangeVfx(bool isMelee)
    {
        if (isMelee)
            return;
        EnterRangeVFX();
    }
    public void ChangeAttackStateOnly(bool isMelee)
    {
        if (isMelee)
        {
            _currentAttack = _meleeAttack;
            EnterMeleeVFX();
        }
        else
        {
            _currentAttack = _rangeAttack;
            EnterRangeVFX();
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
    private void Update()
    {
        if(_PlayerMaster.IsDead())
        {
            return;
        }
        PlayerMod.DoUpdate();
        _currentAttack.DoUpdate();
        if (attackPressed && !prevAttackPressed)
        {
            _currentAttack.StartAttack();
        }
        if (!attackPressed && prevAttackPressed)
        {
            _currentAttack.ReleaseAttack();
        }
        if (skillTrigger)
        {
            skillTrigger = false;
            _closeSkill.StartSkill();
        }
        prevAttackPressed = attackPressed;
    }
    public void EnableDamageBox()
    {
        float dmg = _PlayerMaster._PlayerInstanteState.GetDmg(_currentAttack);
        Vector3 range = _PlayerMaster._PlayerInstanteState.GetRange(_currentAttack);
        _damageBox.EnableDamageBox(dmg, range, OnMeleeHit);
    }
    public void ResetAttack()
    {
        PlayerMod.ResetMod();
        _rangeAttack.ResetAttack();
        _meleeAttack.ResetAttack();
    }
    private void Callback_IsCharged()
    {
        _animator.SetBool("IsCharged", true);
    }
    private void Callback_IsChargedStart()
    {
        _animator.SetBool("IsCharged", false);
    }
    private void OnMeleeHit(int hitCount)
    {
        PlayerInstanteState stat = _PlayerMaster._PlayerInstanteState;
        stat.SkillGaugeRecovery(_currentAttack, hitCount);
        _PlayerMaster.OnMeleeHit();
    }
    private void SetSuperArmorOnAnim()
    {
        _PlayerMaster._PlayerInstanteState.SetSuperArmor(99999999f);
    }
    private void ResetSuperArmorOnAnim()
    {
        _PlayerMaster._PlayerInstanteState.ResetSuperArmor();
    }
    private void SetInvincibleOnAnim()
    {
        _PlayerMaster._PlayerInstanteState.SetInvincible(99999999f);
    }
    private void ResetInvincibleOnAnim()
    {
        if (_PlayerMaster.IsDashing)
            return;
        _PlayerMaster._PlayerInstanteState.ResetInvincible();
    }
    private void ResetEvadeOnAnim()
    {
        if (_PlayerMaster.IsDashing)
            return;
        _PlayerMaster._PlayerInstanteState.ResetEvade();
    }

    public GameObject DashFires;

    public void BlueChipFire()
    {
        int level_blueChip_Generic1 = _PlayerMaster.GetBlueChipLevel(BlueChipID.Generic1);
        if (level_blueChip_Generic1 > 0)//����Ĩ�� 1���� �̻��ϰ��
                                        //0 ����,1������,2���ӽð�,3��������,4����
        {
            BlueChip chip_Generic1 = JsonDataManager.GetBlueChipData(BlueChipID.Generic1);
            float interval = chip_Generic1.Level_VelueList[level_blueChip_Generic1][0];
            float Dmg = chip_Generic1.Level_VelueList[level_blueChip_Generic1][1];
            float fireDotDuration = chip_Generic1.Level_VelueList[level_blueChip_Generic1][2];
            float fireExploDmg = chip_Generic1.Level_VelueList[level_blueChip_Generic1][3];
            float fireExploRadius = chip_Generic1.Level_VelueList[level_blueChip_Generic1][4];
            GameObject _DashFire = ObjectPoolManager.Instance.DequeueObject(DashFires);
            DashFire DF = _DashFire.GetComponent<DashFire>();
            //if (CurrentAttackKind == PlayerAttackKind.RangeNormalAttack)
            if (_currentAttack is PlayerRangeAttack range && level_blueChip_Generic1 < 5)
            {
                DF.transform.position = transform.position;
                DF.transform.rotation = transform.rotation;
                DF.SetStat(Dmg, fireDotDuration, fireExploRadius, fireExploDmg);
                DF.StartFire(DashFire.AttackType.Range);
            }
            else if (level_blueChip_Generic1 == 5)
            {
                DF.transform.position = transform.position;
                DF.transform.rotation = transform.rotation;
                DF.SetStat(Dmg, fireDotDuration, fireExploRadius, fireExploDmg);
                DF.StartFire(DashFire.AttackType.All);
            }
            else
            {
                DF.transform.position = transform.position;
                DF.transform.rotation = transform.rotation;
                DF.SetStat(Dmg, fireDotDuration, fireExploRadius, fireExploDmg);
                DF.StartFire(DashFire.AttackType.Melee);
            }
        }
    }
    private void StartAbsorbingAnimAndVsfx()
    {
        _animator.SetTrigger("Absorbeing");
        _animator.SetBool("AbsorbeingEnd", false);
        _closeSkill.Effect2(startAbsorbing);
        SM.Instance.PlaySound2("Absorbeing", transform.position);
    }
    private void DeactiveAbsortVfxAndAnim()
    {
        _animator.SetTrigger("AbsorbeingEnd");
        ObjectPoolManager.Instance.AllDestroyObject(startAbsorbing.preFab);
    }
    private void ResetMod()
    {
        _animator.SetBool("Absorbeing", false);
        _animator.SetBool("AbsorbeingEnd", false);
        _animator.SetBool("Transform", false);
        ObjectPoolManager.Instance.AllDestroyObject(startAbsorbing.preFab);
    }
}
