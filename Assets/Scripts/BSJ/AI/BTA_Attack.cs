
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class BTA_Attack : Action
    {
        private EnemyAttack _attack;
        [SerializeField] private SharedFloat _range;

        public override void OnAwake()
        {
            EnemyBase owner = GetComponent<EnemyBase>();
            if (owner == null)
            {
                Debug.LogError("no EnemyBase found");
            }
            _attack = owner.Attack;
        }

        public override TaskStatus OnUpdate()
        {
            if (_attack.IsAttacking)
            {
                return TaskStatus.Success;
            }
            _attack.StartAttackAnimation();
            _range.Value = 0f;
            return TaskStatus.Success;
        }
    }
}