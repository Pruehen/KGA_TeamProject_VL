using EnumTypes;
using System;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "CloseAttackModuleData", menuName = "Enemy/AttackModule/CloseAttackModule")]
public class SO_CloseModule : SO_AttackModule
{
    public override void StartAttack(EnemyBase owner, int type)
    {
        base.StartAttack(owner, type);
        owner.Attack.EnableDamageBox();
    }
}
