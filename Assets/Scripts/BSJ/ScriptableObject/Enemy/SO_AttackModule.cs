using EnumTypes;
using System;
using UnityEngine;

[Serializable]
public struct DamageBoxData
{
    public Vector3 Offset;
    public Vector3 Range;

    public DamageBoxData(Vector3 offset, Vector3 range)
    {
        Offset = offset;
        Range = range;
    }
}

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
    public bool IsImmediate = false;
    public float Damage = 10f;
    public DamageBoxData DamageBox = new DamageBoxData(Vector3.zero, Vector3.one);

    public virtual void StartAction(EnemyBase owner)
    {
    }

    public virtual void UpdateAction(EnemyBase owner, float time)
    { }

    public virtual void StartAttack(EnemyBase owner, int type)
    {
    }
    public virtual void UpdateAttack(EnemyBase owner, int type, float time)
    {
        if (Time.time >= owner.Attack.CurrentAttack.PrevAttackTime + AttackTime)
        {
            owner.Attack.CurrentAttack.EndAttack();
        }
    }
    public virtual void StartAttackMove(EnemyBase owner, int type)
    {
        if (owner.Attack.CurrentAttack.IsUpdateMove)
        {
            return;
        }
        owner.Attack.CurrentAttack.IsUpdateMove = true;
    }
    public virtual void UpdateAttackMove(EnemyBase owner, int type, float time)
    {
    }


}
