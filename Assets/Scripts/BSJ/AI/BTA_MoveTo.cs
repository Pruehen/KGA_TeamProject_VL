using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_MoveTo : Action
    {
        public SharedVector3 targetPostion;
        public Detector _detector;
        public NavMeshAgent _agent;
        public bool isDynamicDestination = false;
        public bool isChaseEvenLose = false;

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
            MoveToTarget(_agent, targetPostion.Value);
        }

        public override TaskStatus OnUpdate()
        {
            if (isDynamicDestination)
            {
                if (isChaseEvenLose)
                {
                    targetPostion.Value = _detector.GetLatestTarget().position;
                }
                else
                {
                    targetPostion.Value = _detector.GetTarget().position;
                }
            }
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
            MoveToTarget(_agent, targetPostion.Value);
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

        private void MoveToTarget(NavMeshAgent agent, Vector3 target)
        {
            agent.destination = target;
        }
        private void MoveToTarget2D(NavMeshAgent agent, Vector3 target)
        {
            target.z = 0f;
            agent.destination = target;
        }
    }
}