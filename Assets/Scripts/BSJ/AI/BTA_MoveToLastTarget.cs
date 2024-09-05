using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_MoveToLastTarget : Action
    {
        public Detector _detector;
        public NavMeshAgent _agent;
        public bool isDynamicDestination = false;
        Transform _lastTarget;

        public override void OnAwake()
        {
            EnemyBase owner = GetComponent<EnemyBase>();
            if (owner == null)
            {
                Debug.LogError("no EnemyBase found");
            }
            _agent = owner.NavAgent;
            _detector = owner.Detector;
        }

        public override void OnStart()
        {
            SetMovable(_agent, true);
            MoveToTarget2D(_agent, _detector.GetLastPosition());
        }

        public override TaskStatus OnUpdate()
        {
            //네브메시가 경로 계산중인지 확인 해야함
            if (_agent.pathPending == true)
            {
                return TaskStatus.Running;
            }

            bool isArrived = _agent.remainingDistance <= _agent.stoppingDistance;
            if (isArrived)
            {
                return TaskStatus.Success;
            }
            if(_lastTarget == null)
            {
                return TaskStatus.Failure;
            }
            MoveToTarget2D(_agent, _lastTarget.position);
            return TaskStatus.Running;
        }
        public override void OnEnd()
        {
            _agent.isStopped = true;
        }



        private void SetMovable(NavMeshAgent agent, bool isMovable)
        {
            agent.enabled = isMovable;
            agent.isStopped = !isMovable;
        }

        private void MoveToTarget2D(NavMeshAgent agent, Vector3 target)
        {
            target.z = 0f;
            agent.destination = target;
        }
    }
}