using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExitReset : StateMachineBehaviour
{
    int attackHash;
    PlayerAttackSystem _playerAttack;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackHash = Animator.StringToHash("Attack");
        animator.TryGetComponent(out _playerAttack);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimatorStateInfo nextState = animator.GetNextAnimatorStateInfo(0);
        AnimatorTransitionInfo currentTransition = animator.GetAnimatorTransitionInfo(0);
        if (currentTransition.anyState && nextState.fullPathHash != stateInfo.fullPathHash)
        {
            _playerAttack.ResetAttack();
            
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).tagHash != attackHash)
        {
            _playerAttack.ResetAttack();
        }
    }
}
