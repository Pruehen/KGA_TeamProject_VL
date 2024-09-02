using EnumTypes;
using System;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    PlayerMaster _PlayerMaster;

    Animator _animator;
    PlayerAttack _playerAttack;
    int hashAttackType = Animator.StringToHash("AttackType");
    int hashAttackMod = Animator.StringToHash("AttackMod");
    int hashAttack = Animator.StringToHash("Attack");
    int hashAttackComboInitialIndex = Animator.StringToHash("AttackComboInitialIndex");
    int hasAttackSpeed = Animator.StringToHash("AttackSpeed");
    int hashSkill = Animator.StringToHash("Skill");
    public MeleeAttack CloseAttack;
    Skill _closeSkill;
    [SerializeField] public SO_SKillEvent startAbsorbing;
    [SerializeField] public SO_SKillEvent endAbsorbing;
    [SerializeField] DamageBox _damageBox;
    public void Init(Action onCharged = null, Action onChargeFail = null, Action onChargeEnd = null,Action onChargeStart = null)
    {
        TryGetComponent(out _animator);
        TryGetComponent(out _playerAttack);
        TryGetComponent(out CloseAttack);
        TryGetComponent(out _closeSkill);
        CloseAttack.Init(_animator, onCharged, onChargeEnd, onChargeFail,onChargeStart);
        _closeSkill.Init(_animator);
        _PlayerMaster = GetComponent<PlayerMaster>();

    }



    public bool _attackLcokMove;

    public bool AttackLockMove
    {
        get => _attackLcokMove;
    }
    public bool isAttackTrigger
    {
        get { return _PlayerMaster.isAttackTrigger; }
        set
        {
            _PlayerMaster.isAttackTrigger = value;
        }
    }
    public void StartAttack(PlayerAttackKind mod, PlayerAttackKind index, int comboIndex)
    {
        if (_PlayerMaster.isDashing)
        {
            LockMove();
        }

        _animator.SetTrigger(hashAttack);


        _animator.SetInteger(hashAttackType, (int)index);
        _animator.SetInteger(hashAttackMod, (int)mod);


        _animator.SetInteger(hashAttackComboInitialIndex, comboIndex);
        _animator.SetFloat(hasAttackSpeed, _PlayerMaster._PlayerInstanteState.AttackSpeed);

    }
    public void StartSkill(PlayerAttackKind mod, float skillGauge)
    {
        if (skillGauge >= 100)
        {
            
            _animator.SetInteger(hashAttackMod, (int)mod);
            _animator.SetFloat("SkillGauge", skillGauge);
            _animator.SetTrigger(hashSkill);
            LockMove();
            Debug.Log(skillGauge);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            Debug.Log(skillGauge);
        }
    }
    public void LockMove()
    {
        _attackLcokMove = true;
    }
    public void ReleaseLockMove()
    {
        _attackLcokMove = false;
        Debug.Log("ReleaseLockMove");
    }

    public void OnRelease()
    {
        CloseAttack.EndAttack();
    }
    public void OnReleaseLoop()
    {
        if(!isAttackTrigger)
        {
            _animator.SetTrigger("AttackEnd");
        }
    }
    public void EnableDamageBox(float dmg, Vector3 range, Action OnHitCallback)
    {
        _playerAttack.IncreaseAttackCount();
        _damageBox.EnableDamageBox(dmg, range, OnHitCallback);
    }

    public void ResetEndAttack()
    {
        _animator.ResetTrigger("AttackEnd");
    }
    public void ResetMod()
    {
        _animator.SetTrigger("Reset");
        
    }
    public void Absober()
    {
        Debug.Log("흡수");
        _animator.SetTrigger("Absorbeing");
        _closeSkill.Effect2(startAbsorbing);
    }
    public void AbsoberEnd()
    {
        _animator.SetTrigger("AbsorbeingEnd");
        //_animator.ResetTrigger("Attack");
        //_animator.ResetTrigger("AttackEnd");

        Debug.Log("absoberEnd");
        ObjectPoolManager.Instance.AllDestroyObject(startAbsorbing.preFab);
        //_PlayerMaster._PlayerSkill.Effect2(endAbsorbing);
    }
    public void ModTransform()
    {
        _animator.SetTrigger("Transform");
        _playerAttack.EnterMeleeVFX();
        //_animator.SetTrigger("AbsorbeingEnd");
        //_animator.ResetTrigger("Attack");
        //_animator.ResetTrigger("AttackEnd");
        Debug.Log("Transform");
    }

    public void ResetAttack()
    {
        ReleaseLockMove();
        CloseAttack.ChargeFail();
    }
    public void OnUseSkillGauge()
    {
        _PlayerMaster._PlayerInstanteState.TryUseSkillGauge2();
    }
}
