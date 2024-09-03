using EnumTypes;
using System;

[Serializable]
public class AttackModule
{
    public EnemyBase owner;
    public SO_AttackModule AttackModuleData;
    private bool _available;
    public bool Available { get { return _available; } private set { _available = value; } }

    public bool IsUpdateMove;

    private Timer _timer;

    public AttackModule(EnemyBase enemyBase, SO_AttackModule attackModuleData)
    {
        owner = enemyBase;
        AttackModuleData = attackModuleData;
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
        if(IsUpdateMove)
        {
            AttackModuleData.UpdateAttackMove(deltaTime, owner);
        }
    }
    public virtual bool IsAttackable(AttackRangeType attackRangeType, Phase phase)
    {
        if (attackRangeType == AttackModuleData.AttackRangeType && Available && ((AttackModuleData.Phase & phase) != 0))
        {
            return true;
        }
        return false;
    }
    public virtual void StartAttack()
    {
        _timer.StartTimer();
        Available = false;
    }

    public virtual void StartAttackMove(EnemyBase owner)
    {
        IsUpdateMove = true;
        AttackModuleData.StartAttackMove(owner);
    }
    private void OnCoolEnd()
    {
        Available = true;
    }
}