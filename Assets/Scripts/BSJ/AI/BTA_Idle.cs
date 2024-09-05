namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_Idle : Action
    {
        public EnemyBase owner;

        public override void OnAwake()
        {
            owner = GetComponent<EnemyBase>();
        }

        public override TaskStatus OnUpdate()
        {
            owner.Idle();
            return TaskStatus.Success;
        }
    }
}