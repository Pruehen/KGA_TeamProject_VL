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
    MeleeAttack _closeAttack;
    Skill _closeSkill; 

    [SerializeField] DamageBox _damageBox;
    public void Init(Action onCharged = null, Action onChargeFail = null, Action onChargeEnd = null)
    {
        TryGetComponent(out _animator);
        TryGetComponent(out _playerAttack);
        TryGetComponent(out _closeAttack);
        TryGetComponent(out _closeSkill);
        _closeAttack.Init(_animator, onCharged, onChargeEnd, onChargeFail);
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
        _animator.SetTrigger(hashAttack);


        _animator.SetInteger(hashAttackType, (int)index);
        _animator.SetInteger(hashAttackMod, (int)mod);


        _animator.SetInteger(hashAttackComboInitialIndex, comboIndex);
        _animator.SetFloat(hasAttackSpeed, _PlayerMaster._PlayerInstanteState.AttackSpeed);
    }

    public void StartSkill(int index, float skillGauge)
    {
        if (skillGauge >= 100)
        {
            _animator.SetTrigger(hashSkill);
            _animator.SetInteger(hashAttackType, index);
            _animator.SetFloat("SkillGauge", skillGauge);
            _attackLcokMove = true;
            Debug.Log(skillGauge);
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
        _closeAttack.EndAttack();
    }
    public void OnReleaseLoop()
    {
        if(!isAttackTrigger)
        {
            _animator.SetTrigger("AttackEnd");
        }
    }
    public void EnableDamageBox(float dmg, float range, Action OnHitCallback)
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
    }
    public void AbsoberEnd()
    {
        _animator.SetTrigger("AbsorbeingEnd");
        //_animator.ResetTrigger("Attack");
        //_animator.ResetTrigger("AttackEnd");
        Debug.Log("absoberEnd");
    }
    public void ModTransform()
    {
        _animator.SetTrigger("Transform");
        //_animator.SetTrigger("AbsorbeingEnd");
        //_animator.ResetTrigger("Attack");
        //_animator.ResetTrigger("AttackEnd");
        Debug.Log("Transform");
    }

    public void ResetAttack()
    {
        _attackLcokMove = false;
        _closeAttack.ChargeFail();
        Debug.Log("대머리");
        
    }
    public void OnUseSkillGauge()
    {
        _PlayerMaster._PlayerInstanteState.TryUseSkillGauge2();
    }
}
