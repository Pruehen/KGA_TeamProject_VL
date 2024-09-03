using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Check TargetIsNearOrEnable And Enable")]
    [TaskCategory("Character")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class BTC_IsTargetFar : Conditional
    {
        public Detector _detector;
        public SharedFloat range = 3f;

        public override void OnAwake()
        {
            EnemyBase owner = GetComponent<EnemyBase>();
            if (owner == null)
            {
                Debug.LogError("no EnemyBase found");
            }
            _detector = owner.Detector;
        }
        public override TaskStatus OnUpdate()
        {
            if (_detector == null)
            {
                Debug.LogError("no Detector found");
            }
            if (_detector.IsTargetFar(range.Value))
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
