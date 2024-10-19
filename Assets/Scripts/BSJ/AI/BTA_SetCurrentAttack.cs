using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_SetCurrentAttack : Action
    {
        public EnemyAttack _attack;
        public Detector _detector;
        public SharedFloat _range;

        public override void OnAwake()
        {
            EnemyBase owner = GetComponent<EnemyBase>();
            if (owner == null)
            {
                Debug.LogError("no EnemyBase found");
            }
            _attack = owner.Attack;
            _detector = owner.Detector;
        }

        public override TaskStatus OnUpdate()
        {
            _attack.SetAttackRangeType(_detector.TargetDistance);
            AttackModule curAttack = _attack.SetCurrentAttack(_detector.TargetDistance);
            if (curAttack.AttackModuleData.IsImmediate)
            {
                _range.Value = 999f;
            }
            else
            {
                _range.Value = curAttack.AttackModuleData.AttackRange;
            }
            return TaskStatus.Success;
        }
    }
}