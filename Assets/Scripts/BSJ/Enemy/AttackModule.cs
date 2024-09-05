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
    public AttackRangeType Range { get; internal set; }
    public float PrevAttackTime { get; internal set; }
    public bool IsAttacking { get; private set; }
    public float PrevFireTime { get; internal set; } = 0f;

    public bool IsUpdateMove;
    private bool IsMoveStarted;

    private Timer _timer;
    internal bool hasAttacked;

    public AttackModule(EnemyBase enemyBase, SO_AttackModule attackModuleData)
    {
        owner = enemyBase;
        AttackModuleData = attackModuleData;
        Inited = true;
    }

    public virtual void Init(EnemyBase owner)
    {
        this.owner = owner;
        _timer = new Timer();
        _timer.Init(AttackModuleData.CoolDown, OnCoolEnd);
    }

    public virtual void DoUpdate(float deltaTime)
    {
        _timer.DoUpdate(deltaTime);
    }
    public virtual void DoCurUpdate(float deltaTime)
    {
        if (IsUpdateMove && IsMoveStarted)
        {
            AttackModuleData.UpdateAttackMove(deltaTime, owner);
        }
        if (IsAttacking)
            AttackModuleData.UpdateAttack(deltaTime, owner);
    }
    public virtual bool IsAttackable(AttackRangeType attackRangeType, Phase phase)
    {
        if ((attackRangeType & AttackModuleData.AttackRangeType) != 0 && Available && ((AttackModuleData.Phase & phase) != 0))
        {
            Range = attackRangeType;
            return true;
        }
        return false;
    }
    public virtual void StartAttack()
    {
        _timer.StartTimer();
        Available = false;
        IsMoveStarted = false;
        PrevAttackTime = Time.time;
        Debug.Log(Time.time);
    }

    public virtual void StartAttackModulAction(EnemyBase owner)
    {
        AttackModuleData.StartAttack(owner);
        IsAttacking = true;
    }

    public virtual void StartAttackMove(EnemyBase owner)
    {
        IsUpdateMove = true;
        IsMoveStarted = true;
        AttackModuleData.StartAttackMove(owner);
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