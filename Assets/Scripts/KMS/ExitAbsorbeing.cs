using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitAbsorbeing : StateMachineBehaviour
{


    [SerializeField] AttackSystem _playerAttack;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerAttack.LockMove();
        Debug.Log("Exit");
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.TryGetComponent(out _playerAttack);

    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKey(KeyCode.Tab))
            Debug.Break();
    }
}
