using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    Animator _animator;
    int hashAttackType = Animator.StringToHash("AttackType");
    int hashAttack = Animator.StringToHash("Attack");
    void Start()
    {
        TryGetComponent(out _animator);
    }
    public int AttackIndex
    {
        get => _animator.GetInteger(hashAttackType);
        set => _animator.SetInteger(hashAttackType, value);
    }

    public void Attack(int index)
    {
        _animator.SetTrigger(hashAttack);
        _animator.SetInteger(hashAttackType, index);
    }
}
