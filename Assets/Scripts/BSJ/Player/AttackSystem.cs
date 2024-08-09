using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    Animator _animator;
    int hashAttackType = Animator.StringToHash("AttackType");
    int hashAttack = Animator.StringToHash("Attack");

    bool _attackLcokMove;

    public bool AttackLockMove
    {
        get => _attackLcokMove;
    }
    void Start()
    {
        TryGetComponent(out _animator);
    }
    public int AttackIndex
    {
        get => _animator.GetInteger(hashAttackType);
        set => _animator.SetInteger(hashAttackType, value);
    }

    public void StartAttack(int index)
    {
        _attackLcokMove = true;
        _animator.SetTrigger(hashAttack);
        _animator.SetInteger(hashAttackType, index);
    }

    public void LockMove()
    {
        _attackLcokMove = true;
    }
    public void ReleaseLockMove()
    {
        _attackLcokMove = false;
    }
}
