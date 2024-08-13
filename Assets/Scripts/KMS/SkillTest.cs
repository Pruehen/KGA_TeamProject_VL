using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class Skill : MonoBehaviour
{

    InputManager _InputManager;
    Animator _animator;

    int _animTriggerAttack;
    int _animBoolChargingEnd;
    int _animTriggerAttackEnd;
    bool _isAttacking;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Init(_animator);
    }
    public void Init(Animator animator)
    {
        _animator = animator;
        _animTriggerAttack = Animator.StringToHash("Attack");
        _animBoolChargingEnd = Animator.StringToHash("ChargingEnd");
        _animTriggerAttackEnd = Animator.StringToHash("AttackEnd");
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
}
