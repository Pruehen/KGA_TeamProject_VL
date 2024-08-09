using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseAttack : MonoBehaviour
{
    Animator _animator;

    int _startAttackTrigger;
    int _endAttackTrigger;

    public void Init(Animator animator)
    {
        _animator = animator;
        _startAttackTrigger = Animator.StringToHash("StartCloseAttack");
        _endAttackTrigger = Animator.StringToHash("EndCloseAttack");
    }

    public void StartAttack()
    {
        _animator.SetTrigger(_startAttackTrigger);
    }

    public void EndAttack()
    {
        _animator.SetTrigger(_endAttackTrigger);
    }

}
