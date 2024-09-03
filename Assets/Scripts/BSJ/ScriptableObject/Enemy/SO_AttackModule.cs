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

    public virtual void StartAttackMove(EnemyBase owner)
    {
    }
    public virtual void UpdateAttackMove(float time, EnemyBase owner)
    {
    }
}
