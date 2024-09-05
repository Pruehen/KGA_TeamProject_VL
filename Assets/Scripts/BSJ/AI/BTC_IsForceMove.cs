using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Check IsEnemy ForceMove")]
    [TaskCategory("Character")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class BTC_IsForceMove : Conditional
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

            if (owner.Move.IsForceMove)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}
