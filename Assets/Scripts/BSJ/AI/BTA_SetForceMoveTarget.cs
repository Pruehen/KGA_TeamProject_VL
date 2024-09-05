using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_SetForceMoveTarget : Action
    {
        public SharedVector3 targetPostion;
        EnemyBase _owner;

        public override void OnAwake()
        {
            _owner = GetComponent<EnemyBase>();
            if (_owner == null)
            {
                Debug.LogError("no EnemyBase found");
            }
        }

        public override TaskStatus OnUpdate()
        {
            Vector3 target = _owner.Move.ForceMoveTarget;

            if (target != Vector3.zero)
            {
                targetPostion.Value = target;
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}