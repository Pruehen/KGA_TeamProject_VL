namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_Attack : Action
    {
        public Enemy owner;

        public override void OnAwake()
        {
            owner = GetComponent<Enemy>();
        }

        public override TaskStatus OnUpdate()
        {
            if (owner.IsDead()) 
            {
                return TaskStatus.Failure;
            }
            owner.StartAttackAnimation();
            return TaskStatus.Success;
        }
    }
}