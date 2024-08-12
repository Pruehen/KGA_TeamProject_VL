using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class ATKTest : MonoBehaviour
{
    
    InputManager _InputManager;
    Animator _animator;

    int _animTriggerAttack;
    int _animBoolChargingEnd;
    int _animTriggerAttackEnd;
    int _animTriggerDashEnd;
    int _animTriggerDash;
    bool _isAttacking;

    private void Awake()
    {
        _animator= GetComponent<Animator>();
        Init(_animator);
    }
    public void Init(Animator animator)
    {
        _animator = animator;
        _animTriggerAttack = Animator.StringToHash("Attack");
        _animBoolChargingEnd = Animator.StringToHash("ChargingEnd");
        _animTriggerAttackEnd = Animator.StringToHash("AttackEnd");
        _animTriggerDashEnd = Animator.StringToHash("DashEnd");
        _animTriggerDash = Animator.StringToHash("Dash");
    }
    public void EndAttack()
    {
        _animator.SetTrigger(_animTriggerAttackEnd);
    }

    public void ATKEnd()
    {
        if (!_animator.GetBool(_animTriggerAttack))
        {
            _animator.SetTrigger(_animTriggerAttackEnd);
            Debug.Log("ATKEnd");
        }
    }
    public void DashEnd()
    {
        _animator.SetTrigger(_animTriggerDashEnd);
        Debug.Log("DashEnd");
    }
    public void DashAtkEnd()
    {
        _animator.ResetTrigger(_animTriggerAttackEnd);
        _animator.ResetTrigger(_animTriggerDashEnd);
        _animator.ResetTrigger(_animTriggerDash);

    }
}
