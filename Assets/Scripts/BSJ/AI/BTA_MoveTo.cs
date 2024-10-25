using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_MoveTo : Action
    {
        public SharedVector3 targetPostion;
        public SharedTransform targetTransform;
        public NavMeshAgent _agent;
        public bool isDynamicDestination = false;

        public override void OnAwake()
        {
             _agent = GetComponent<NavMeshAgent>();
            if (_agent == null)
            {
                Debug.LogError("no _agent found");
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (_agent.pathPending == true)
            {
                return TaskStatus.Running;
            }

            Vector3 target;
            if (isDynamicDestination)
            {
                target = targetTransform.Value.position;
            }
            else
            {
                target = targetPostion.Value;
            }

            MoveToTarget(_agent, target);

            if(!_agent.isOnNavMesh)
            {
                return TaskStatus.Failure;
            }
            bool isArrived = _agent.remainingDistance <= _agent.stoppingDistance;
            if (isArrived)
            {
                _agent.isStopped = true;
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
        public override void OnEnd()
        {
            base.OnEnd();
            if(_agent == null || !_agent.isOnNavMesh)
            {
                return;
            }
            _agent.isStopped = true;
        }

        private void MoveToTarget(NavMeshAgent agent, Vector3 target)
        {
            if (agent.pathPending)
            {
                return;
            }
            if(!agent.isOnNavMesh)
            {
                return;
            }
            _agent.isStopped = false;
            agent.destination = target;
        }
    }
}