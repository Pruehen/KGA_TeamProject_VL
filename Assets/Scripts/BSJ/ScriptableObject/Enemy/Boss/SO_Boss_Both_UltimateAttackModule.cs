using EnumTypes;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Boss_Both_UltimateAttackModule", menuName = "Enemy/AttackModule/Boss_Both_UltimateAttack")]
public class SO_Boss_Both_UltimateAttackModule : SO_RangeModule
{
    public override void StartAttack(EnemyBase owner, int type)
    {
        //base.StartAttack(owner, type);

        Transform targetTrf = owner.transform;
        Vector3 targetPos = targetTrf.position;
        targetPos.y = 0f;
        GameObject projectileObject = GameObject.Instantiate(Prefab_projectile,
            targetPos, Quaternion.identity);
        projectileObject.GetComponent<BossDoubleAreaAttack>().Init(owner.Attack.CurrentAttack.Range);
    }
}
