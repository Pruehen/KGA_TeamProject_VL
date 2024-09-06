using EnumTypes;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Boss_Range_SpawnSpikeAttackModule", menuName = "Enemy/AttackModule/Boss_Range_SpawnSpikeAttack")]
public class SO_Boss_Range_SpawnSpikeAttackModule : SO_RangeModule
{
    public override void StartAttack(EnemyBase owner, int type)
    {
        //base.StartAttack(owner, type);

        owner.Attack.EnableDamageBox();

        Transform targetTrf = owner.Detector.GetLatestTarget();
        Vector3 targetPos = targetTrf.position;
        targetPos.y = 0f;
        GameObject projectileObject = GameObject.Instantiate(Prefab_projectile,
            targetPos, Quaternion.identity);
    }
}
