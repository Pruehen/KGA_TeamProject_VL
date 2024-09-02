using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "BossAttackModuleData", menuName = "Boss/AttackModule/AttackBase")]
public class BossAttackModule : ScriptableObject
{
    private BossAttackRangeType _attackRangeType;
    private int _phase;

    public int Id { get; internal set; }
    private bool _available;
    private float _coolDown;
    public bool Available { get { return _available; } private set { _available = value; } }

    [SerializeField] private float _attackRange;
    public float AttackRange { get; internal set; }

    private Timer _timer;

    public void Init()
    {
        _timer.Init(_coolDown, OnCoolEnd);
    }

    public void DoUpdate(float deltaTime)
    {
        _timer.DoUpdate(deltaTime);

    }
    public bool IsAttackable(BossAttackRangeType attackRangeType, int phase)
    {
        if(attackRangeType == _attackRangeType && Available && _phase <= phase)
        {
            return true;
        }
        return false;
    }
    public void StartAttack()
    {
        _timer.StartTimer();
        Available = false;
    }


    private void OnCoolEnd()
    {
        Available = true;
    }
}
