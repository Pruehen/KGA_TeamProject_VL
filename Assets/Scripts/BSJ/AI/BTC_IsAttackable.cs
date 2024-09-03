using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Check IsEnemy Attackable")]
    [TaskCategory("Character")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class BTC_IsAttackable : Conditional
    {
        public EnemyBase owner;

        public override void OnAwake()
        {
            owner = GetComponent<EnemyBase>();
        }
        public override TaskStatus OnUpdate()
        {
            if (owner == null)
            {
                Debug.LogError("Unable to find field - value is null");
            }

            if (owner.IsAttackable())
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}
