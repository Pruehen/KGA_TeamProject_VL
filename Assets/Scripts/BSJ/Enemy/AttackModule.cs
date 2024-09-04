using EnumTypes;
using System;

[Serializable]
public class AttackModule
{
    public EnemyBase owner;
    public SO_AttackModule AttackModuleData;
    private bool _available = true;
    public bool Available { get { return _available; } private set { _available = value; } }

    public bool Inited { get; internal set; }

    public bool IsUpdateMove;
    private bool IsMoveStarted;

    private Timer _timer;
    private float _prevAttackTime;
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
        AttackModuleData.UpdateAttack(deltaTime, owner, ref _prevAttackTime);
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
        IsMoveStarted = false;
    }

    public virtual void StartAttackModulAction(EnemyBase owner)
    {
        AttackModuleData.StartAttack(owner);
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
}