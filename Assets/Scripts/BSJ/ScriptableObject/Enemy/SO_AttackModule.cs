using EnumTypes;
using System;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "AttackModuleData", menuName = "Enemy/AttackModule/AttackBase")]
public class SO_AttackModule : ScriptableObject
{
    public int Id;
    public Phase Phase;
    public AttackRangeType AttackRangeType;
    public float AttackRange;
    public float CoolDown;
    public int Priority;
    public float AttackTime = .5f;
    public bool IsAttackType = true;


    public virtual void StartAction(EnemyBase owner)
    {
    }

    public virtual void UpdateAction(EnemyBase owner)
    { }

    public virtual void StartAttack(EnemyBase owner)
    {
        owner.Attack.EnableDamageBox();
    }
    public virtual void UpdateAttack(float time, EnemyBase owner)
    {
        if(Time.time >= owner.Attack.CurrentAttack.PrevAttackTime + AttackTime)
        {
            owner.Attack.CurrentAttack.EndAttack();
        }
    }
    public virtual void StartAttackMove(EnemyBase owner)
    {
    }
    public virtual void UpdateAttackMove(float time, EnemyBase owner)
    {
    }
}
