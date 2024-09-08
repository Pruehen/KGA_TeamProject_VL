using EnumTypes;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Boss_Both_UltimateAttackModule", menuName = "Enemy/AttackModule/Boss_Both_UltimateAttack")]
public class SO_Boss_Both_UltimateAttackModule : SO_AttackModule
{
    public GameObject Prefab_projectile;
    public override void StartAttack(EnemyBase owner, int type)
    {
        switch (type)
        {
            case 0:
                Transform targetTrf = owner.transform;
                Vector3 targetPos = targetTrf.position;
                targetPos.y = 0f;
                GameObject projectileObject = GameObject.Instantiate(Prefab_projectile,
                    targetPos, Quaternion.identity);
                BossDoubleAreaAttack areaAttack = projectileObject.GetComponent<BossDoubleAreaAttack>();
                areaAttack.Init(Damage, owner.Attack.CurrentAttack.Range);
                owner.Attack.CurrentProjectile = areaAttack;
                break;
            case 1:
                owner.Attack.CurrentProjectile.Trigger();
                break;
        }

    }
}
