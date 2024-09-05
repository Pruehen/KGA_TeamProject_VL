using EnumTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAttack
{
    [SerializeField] private SO_AttackModule[] _modulesSetupData;
    [SerializeField] private SO_AttackModule _defaultAttackSetupData;
    [SerializeField] private float _rangeTypeThreshold = 15f;
    [SerializeField] private bool _isPriorityAttack = false;
    [SerializeField] private Transform _firePos;

    private EnemyBase _owner;
    [SerializeField] private AttackModule[] _modules;
    private AttackModule _defaultAttack;
    private DamageBox _attackCollider;

    private float _attackDamage;
    private float _colDamage;

    float _attackSpeedMulti = 1f;

    private bool _isAnimAttacking;
    private bool _isAttackAnim;
    public bool IsAttacking
    {
        get
        {
            return _isAttackAnim ||
                _isAnimAttacking ||
                (CurrentAttack?.IsAttacking ?? false);
        }
    }

    private Phase _phase = Phase.First;
    private AttackRangeType _attackRangeType = AttackRangeType.Close;


    private AttackModule _currentAttack;
    public AttackModule CurrentAttack => _currentAttack;

    private Animator _animator;
    float AttackSpeedMulti
    {
        get
        {
            return _attackSpeedMulti;
        }
        set
        {
            _attackSpeedMulti = value;
            _animator.SetFloat("AttackSpeed", value);
        }
    }

    public void Init(EnemyBase owner, DamageBox damageBox, SO_EnemyBase _enemyData, Animator animator)
    {
        _owner = owner;
        _animator = animator;
        _attackCollider = damageBox;
        _attackDamage = _enemyData.AttackDamage;
        AttackSpeedMulti = _enemyData.AttackSpeedMultiply;

        _defaultAttack = new AttackModule(_owner, _defaultAttackSetupData);
        _defaultAttack.Init(_owner);
        _modules = new AttackModule[_modulesSetupData.Length];

        for (int i = 0; i < _modulesSetupData.Length; i++)
        {
            _modules[i] = new AttackModule(_owner, _modulesSetupData[i]);
            _modules[i].Init(_owner);
        }
    }
    public void DoUpdate(float deltaTime)
    {
        foreach (AttackModule am in _modules)
        {
            am.DoUpdate(deltaTime);
        }
        if (_currentAttack != null && _currentAttack.Inited)
        {
            _currentAttack.DoCurUpdate(deltaTime);
        }
        if (AnimatorHelper.IsAnimationPlaying_Tag(_animator, 0, "Attack"))
        {
            _isAttackAnim = true;
        }
        else
        {
            if (_isAttackAnim)
            {
                OnAnimEnd();
            }
            _isAttackAnim = false;
        }
    }

    public virtual void SetAttackRangeType(float dist)
    {
        if (dist > _rangeTypeThreshold)
        {
            _attackRangeType = AttackRangeType.Far;
        }
        else
        {
            _attackRangeType = AttackRangeType.Close;
        }
    }
    public void SetAttackPhaseType(Phase phase)
    {
        _phase = phase;
    }
    public AttackModule GetRandomAvailableAttack(float dist)
    {
        AttackModule am = null;
        if (_isPriorityAttack)
        {
            am = TryGetPriorityAttack();
        }
        else
        {
            am = TryGetRandomAvailableAttack(dist);
        }
        if (am == null)
        {
            _currentAttack = _defaultAttack;
            return _defaultAttack;
        }
        _currentAttack = am;
        return am;
    }

    private AttackModule TryGetPriorityAttack()
    {
        List<AttackModule> availableAttacks = new List<AttackModule>();

        foreach (AttackModule attack in _modules)
        {
            if (attack.IsAttackable(_attackRangeType, _phase))
            {
                availableAttacks.Add(attack);
            }
        }
        if (availableAttacks.Count == 0)
        {
            return null;
        }

        availableAttacks.Sort((l, r) => { return l.AttackModuleData.Priority.CompareTo(r.AttackModuleData.Priority); });

        List<AttackModule> samePriority = new List<AttackModule>();
        int p = availableAttacks[0].AttackModuleData.Priority;
        foreach (AttackModule attack in availableAttacks)
        {
            if (attack.AttackModuleData.Priority == p)
            {
                samePriority.Add(attack);
            }
        }

        _currentAttack = samePriority[UnityEngine.Random.Range(0, samePriority.Count)];
        return _currentAttack;
    }
    private AttackModule TryGetRandomAvailableAttack(float dist)
    {
        List<AttackModule> availableAttacks = new List<AttackModule>();
        AttackModule lastAttack = null;
        foreach (AttackModule attack in _modules)
        {
            if (attack.IsAttackable(_attackRangeType, _phase) && dist < attack.AttackModuleData.AttackRange)
            {
                availableAttacks.Add(attack);
            }
            lastAttack = attack;
        }

        if (availableAttacks.Count == 0)
        {
            return lastAttack;
        }

        _currentAttack = availableAttacks[UnityEngine.Random.Range(0, availableAttacks.Count)];
        return _currentAttack;
    }

    public void StartAttackAnimation()
    {
        _isAnimAttacking = true;
        _animator.SetInteger("AttackId", _currentAttack.AttackModuleData.Id);
        _animator.SetTrigger("Attack");
        _currentAttack.StartAttack();
    }
    public void StartModulAction()
    {
        _currentAttack.StartAttackModulAction(_owner);
    }
    public void StartAttackMove(int type)
    {
        _currentAttack.StartAttackMove(_owner);
        return;
    }
    public void EnableDamageBox()
    {
        if (_attackCollider == null)
            return;
        _attackCollider.EnableDamageBox(_attackDamage);
    }

    private void OnAnimEnd()
    {
        _isAnimAttacking = false;
    }

    public void OnCollisionStay(Collision collision)
    {
    }
}