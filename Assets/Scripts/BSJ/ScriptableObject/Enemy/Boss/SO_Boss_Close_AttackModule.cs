using EnumTypes;
using System;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Boss_Close_AttackModuleData", menuName = "Enemy/AttackModule/Boss_Close_Attack")]
public class SO_Boss_Close_AttackModule : SO_AttackModule
{
    public float AttackMoveSpeed = 20f;
    public override void StartAction(EnemyBase owner)
    {
        base.StartAction(owner);
        owner.Move.MoveSpeed = AttackMoveSpeed;
    }
    public override void StartAttack(EnemyBase owner, int type)
    {
        base.StartAttack(owner, type);
        owner.Attack.EnableDamageBox( Damage,DamageBox.Offset,DamageBox.Range);

        owner.Move.ResetMoveSpeed();
    }
}
