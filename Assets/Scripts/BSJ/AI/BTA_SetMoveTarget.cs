using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_SetMoveTarget : Action
    {
        public SharedVector3 targetPosition;
        public SharedTransform target;
        public Detector _detector;
        public bool isChaseEvenLost = false;

        public override void OnAwake()
        {
            EnemyBase owner = GetComponent<EnemyBase>();
            if (owner == null)
            {
                Debug.LogError("no EnemyBase found");
            }
            _detector = owner.Detector;
        }

        public override TaskStatus OnUpdate()
        {
            if (_detector == null)
            {
                Debug.LogError("no Detector found");
            }
            if(isChaseEvenLost)
            {
                Transform t = _detector.GetLatestTarget();
                target.Value = t;
                targetPosition.Value = t.position;
            }
            else
            {
                Transform t = _detector.GetTarget();
                target.Value = t;
                targetPosition.Value = t.position;
            }
            return TaskStatus.Success;
        }
    }
}