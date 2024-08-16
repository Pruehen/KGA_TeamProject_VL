using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorResetTrigger : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("triggerName");
        animator.ResetTrigger("AttackEnd");
        animator.ResetTrigger("DashEnd");
        animator.ResetTrigger("Skill");
        animator.ResetTrigger("Absorbeing");
        animator.ResetTrigger("AbsorbeingEnd");
        animator.ResetTrigger("Transform");
        
        Debug.Log("reset");
    }
}
