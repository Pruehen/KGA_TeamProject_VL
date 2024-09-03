using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Check Is Enemy can see target")]
    [TaskCategory("Character")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class BTC_IsVisible : Conditional
    {
        public Detector _detector;
        public bool isAttack;

        public override void OnAwake()
        {
            EnemyBase owner = GetComponent<EnemyBase>();
            if (owner == null)
            {
                Debug.LogError("noEnemyBase found");
            }
            _detector = owner.Detector;
        }

        public override TaskStatus OnUpdate()
        {
            if (_detector == null)
            {
                Debug.LogError("no Detector found");
            }
            if(_detector.IsTargetVisible())
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
