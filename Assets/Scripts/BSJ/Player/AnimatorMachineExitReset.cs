using UnityEngine;

public class AnimatorMachineExitReset : StateMachineBehaviour
{
    [SerializeField] string triggerName;

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.ResetTrigger(triggerName);
        Debug.Log("ResetAfterMachinEnter");
    }
}
