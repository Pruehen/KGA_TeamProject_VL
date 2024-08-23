using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEnterSkill : StateMachineBehaviour
{
    [SerializeField]AttackSystem _playerAttack;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.ReleaseLockMove();
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.TryGetComponent(out _playerAttack);
     
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
            _playerAttack.LockMove();
    }
}
