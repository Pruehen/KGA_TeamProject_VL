using EnumTypes;
using System;
using UnityEngine;

[Serializable]
public class AttackModule
{
    public EnemyBase owner;
    public SO_AttackModule AttackModuleData;
    private bool _available = true;
    public bool Available { get { return _available; } private set { _available = value; } }

    public bool Inited { get; internal set; }
    public AttackRangeType RangeType { get; internal set; }
    public float PrevAttackTime { get; internal set; }
    public bool IsAttacking { get; private set; }
    public float PrevFireTime { get; internal set; } = 0f;

    public bool IsUpdateMove;
    private bool IsMoveStarted;

    public float PrevMoveTime { get; private set; }

    private Timer _timer;
    internal bool hasAttacked;
    private int _moveType;
    private int _attackType;

    public virtual void Init(EnemyBase owner, SO_AttackModule attackModuleData)
    {
        this.owner = owner;
        AttackModuleData = attackModuleData;
        Inited = true;
        _timer = new Timer();
        _timer.Init(AttackModuleData.CoolDown, OnCoolEnd);
    }

    public virtual void DoUpdate(float deltaTime)
    {
        _timer.DoUpdate(deltaTime);
    }
    public virtual void DoCurUpdate(float deltaTime)
    {
        if (this != owner.Attack.CurrentAttack)
            return;
        if (IsUpdateMove && IsMoveStarted)
            AttackModuleData.UpdateAttackMove(owner,_moveType, deltaTime);
        if (IsAttacking)
            AttackModuleData.UpdateAttack(owner,_attackType, deltaTime);
        AttackModuleData.UpdateAction(owner, deltaTime);
    }
    public virtual bool IsAttackable(AttackRangeType attackRangeType, Phase phase)
    {
        if(AttackModuleData.IsImmediate)
        {
            if(owner.Detector.TargetDistance > AttackModuleData.AttackRange  )
            {
                return false;
            }
        }
        if ((attackRangeType & AttackModuleData.AttackRangeType) != 0 && Available && ((AttackModuleData.Phase & phase) != 0))
        {
            RangeType = attackRangeType;
            return true;
        }
        return false;
    }

    public void StartAction(EnemyBase owner)
    {
        Available = false;
        IsMoveStarted = false;
        IsAttacking = false;
        AttackModuleData.StartAction(owner);
        _timer.StartTimer();
    }
    public void StartAttack(EnemyBase owner, int type)
    {
        if (IsAttacking)
        {
            Debug.Log("Already Attacking");
            return;
        }
        _attackType = type;

        IsAttacking = true;
        PrevAttackTime = Time.time;
        AttackModuleData.StartAttack(owner, type);
    }
    public void StartAttackMove(EnemyBase owner, int type)
    {
        _moveType = type;

        IsMoveStarted = true;
        PrevMoveTime = Time.time;
        AttackModuleData.StartAttackMove(owner, type);
    }

    private void OnCoolEnd()
    {
        Available = true;
    }
    public void EndAttack()
    {
        IsAttacking = false;
        PrevFireTime = 0f;
    }
}