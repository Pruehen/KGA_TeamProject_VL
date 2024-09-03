using EnumTypes;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_SetState : Action
    {
        public EnemyBase enemy;
        public AIState state;

        public override void OnAwake()
        {
            base.OnAwake();
            enemy = GetComponent<EnemyBase>();
        }

        public override TaskStatus OnUpdate()
        {
            enemy.SetState(state);
            return TaskStatus.Success;
        }
    }

}