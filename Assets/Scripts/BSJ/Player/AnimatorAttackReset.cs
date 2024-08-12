using UnityEngine;

public class AnimatorAttackReset : StateMachineBehaviour
{
    [SerializeField] string triggerName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(triggerName);
    }
}
