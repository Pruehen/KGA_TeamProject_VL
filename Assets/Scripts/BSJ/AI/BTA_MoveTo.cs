using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_MoveTo : Action
    {
        public SharedVector3 targetPostion;
        public NavMeshAgent agent;
        public Enemy enemy;
        public bool isDynamicDestination = false;
        public bool isChaseEvenLose = false;

        public override void OnAwake()
        {
            agent = GetComponent<NavMeshAgent>();
            enemy = GetComponent<Enemy>();
        }

        public override void OnStart()
        {
            SetMovable(agent, true);
            MoveToTarget(agent, targetPostion.Value);
        }

        public override TaskStatus OnUpdate()
        {
            if (isDynamicDestination)
            {
                if (isChaseEvenLose)
                {
                    targetPostion.Value = enemy.GetTargetPositionAlways();
                }
                else
                {
                    targetPostion.Value = enemy.GetTargetPosition();
                }
            }
            //�׺�޽ð� ��� ��������� Ȯ�� �ؾ���
            if (agent.pathPending == true)
            {
                return TaskStatus.Running;
            }

            bool isArrived = agent.remainingDistance <= agent.stoppingDistance;
            if (isArrived)
            {
                return TaskStatus.Success;
            }
            MoveToTarget(agent, targetPostion.Value);
            return TaskStatus.Running;
        }
        public override void OnEnd()
        {
            agent.isStopped = true;
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