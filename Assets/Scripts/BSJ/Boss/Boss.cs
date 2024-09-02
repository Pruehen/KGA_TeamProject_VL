using System;
using UnityEngine;
using UnityEngine.AI;

public enum BossAttackRangeType
{
    Close,
    Far
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Boss : EnemyBase
{
    public BossAttack _bossAttack;

    [SerializeField] private Detector _detector;
    [SerializeField] private BossAttackModule _curAttack;
    [SerializeField] private float _distanceThreshold = 30f;

    [SerializeField] private BossAttackRangeType _attackRange;

    public Combat combat_phase2;
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    public void SetAttackType()
    {
        if (_detector.GetLatestTarget() == null)
        {
            Debug.Assert(false, "it cannot be null");
        }
        if (_detector.TargetDistance > _distanceThreshold)
        {
            _attackRange = BossAttackRangeType.Far;
        }
        else
        {
            _attackRange = BossAttackRangeType.Close;
        }

        _bossAttack.SetAttackRangeType(_attackRange);
        _curAttack = _bossAttack.TryGetRandomAvailableAttack();

        if (_curAttack != null)
        {
            SetChaseRange(_curAttack.AttackRange);
        }
    }

    private void SetChaseRange(object attackRange)
    {
        throw new NotImplementedException();
    }

    public void ExcuteAttackAnim()
    {
        _anim.SetInteger("AttackType", (int)_curAttack.Id);
        _anim.SetTrigger("Attack");
    }

    public void OnPhase1Dead()
    {

    }

    public void GetDamageColliderInfo()
    {
        _bossAttack.SetAttackPhaseType(1);
    }
}
