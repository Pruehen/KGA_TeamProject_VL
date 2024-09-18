using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEnterSkill : StateMachineBehaviour
{
    [SerializeField]PlayerAttackSystem _playerAttack;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.TryGetComponent(out _playerAttack);
     
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
    }
}
