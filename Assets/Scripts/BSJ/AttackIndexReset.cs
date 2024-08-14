using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndexReset : StateMachineBehaviour
{
    int attackHash;
    PlayerAttack _playerAttack;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackHash = Animator.StringToHash("Attack");
        animator.TryGetComponent(out _playerAttack);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetAnimatorTransitionInfo(0).anyState)
        {
            _playerAttack.ResetAttackCount();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).tagHash != attackHash)
        {
            _playerAttack.ResetAttackCount();
        }
    }
}
