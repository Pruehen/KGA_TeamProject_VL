using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorResetTrigger : StateMachineBehaviour
{
    public bool Attak;
    public bool triggerName;
    public bool AttackEnd;
    public bool DashEnd;
    public bool Skill;
    public bool Absorbeing;
    public bool AbsorbeingEnd;
    public bool Transform;







    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Attak)
        animator.ResetTrigger("Attack");
        if(triggerName)
        animator.ResetTrigger("triggerName");
        if(AttackEnd)
        animator.ResetTrigger("AttackEnd");
        if(DashEnd)
        animator.ResetTrigger("DashEnd");
        if(Skill)
        animator.ResetTrigger("Skill");
        if(Absorbeing)
        animator.ResetTrigger("Absorbeing");
        if(AbsorbeingEnd)
        animator.ResetTrigger("AbsorbeingEnd");
        if(Transform)
        animator.ResetTrigger("Transform");
        Debug.Log("reset");
    }
}
