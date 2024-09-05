namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_SetBossAttackType: Action
    {
        public Boss _boss;

        public override void OnAwake()
        {
            base.OnAwake();
            _boss = GetComponent<Boss>();
        }

        public override TaskStatus OnUpdate()
        {
            _boss.SetAttackType();
            return TaskStatus.Success;
        }
    }

}