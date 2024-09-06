using EnumTypes;
using System;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Boss_Close_AttackModuleData", menuName = "Enemy/AttackModule/Boss_Close_Attack")]
public class SO_Boss_Close_AttackModule : SO_AttackModule
{
    public override void StartAttack(EnemyBase owner, int type)
    {
        base.StartAttack(owner, type);
        owner.Attack.EnableDamageBox( Damage,DamageBox.Offset,DamageBox.Range);
    }
}
