namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_Idle : Action
    {
        public Enemy owner;

        public override void OnAwake()
        {
            owner = GetComponent<Enemy>();
        }

        public override TaskStatus OnUpdate()
        {
            owner.Idle();
            return TaskStatus.Success;
        }
    }
}