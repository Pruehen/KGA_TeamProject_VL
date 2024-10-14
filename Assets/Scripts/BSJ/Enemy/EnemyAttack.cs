using EnumTypes;
using System;
using System.Collections.Generic;
using UnityEngine;
using BSJ.Sort;

[Serializable]
public class EnemyAttack
{
    [SerializeField] internal SO_AttackModule[] _modulesSetupData;
    [SerializeField] internal SO_AttackModule _defaultAttackSetupData;
    [SerializeField] private float _rangeTypeThreshold = 15f;
    [SerializeField] private bool _isPriorityAttack = false;
    [SerializeField] private Transform _firePos;
    public float RangeTypeThreshold => _rangeTypeThreshold;

    internal EnemyBase _owner;
    [SerializeField] internal AttackModule[] _modules;
    internal AttackModule _defaultAttack;
    internal DamageBox _attackCollider;

    internal float _colDamage;

    float _attackSpeedMulti = 1f;

    internal bool _isAnimAttacking;
    internal bool _isAttackAnim;
    public bool IsAttacking
    {
        get
        {
            return _isAttackAnim ||
                _isAnimAttacking ||
                (CurrentAttack?.IsAttacking ?? false);
        }
    }

    internal Phase _phase = Phase.First;
    internal AttackRangeType _attackRangeType = AttackRangeType.Close;


    internal AttackModule _currentAttack;
    public AttackModule CurrentAttack => _currentAttack;

    internal Animator _animator;
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

    public List<SpikeSpawner> CurrentSpikeSpawners;

    public BossDoubleAreaAttack CurrentProjectile { get; internal set; }

    public void Init(EnemyBase owner, DamageBox damageBox, SO_EnemyBase _enemyData, Animator animator)
    {
        _owner = owner;
        _animator = animator;
        _attackCollider = damageBox;
        AttackSpeedMulti = _enemyData.AttackSpeedMultiply;

        _defaultAttack = new AttackModule();
        _defaultAttack.Init(_owner, _defaultAttackSetupData);
        _modules = new AttackModule[_modulesSetupData.Length];

        for (int i = 0; i < _modulesSetupData.Length; i++)
        {
            _modules[i] = new AttackModule();
            _modules[i].Init(_owner, _modulesSetupData[i]);
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
        if (AnimatorHelper.IsAnimCur_Tag(_animator, 0, "Attack"))
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
    public virtual AttackRangeType GetAttackRangeType()
    {
        float dist = _owner.Detector.TargetDistance;
        if (dist > _rangeTypeThreshold)
        {
            return AttackRangeType.Far;
        }
        else
        {
            return AttackRangeType.Close;
        }
    }
    public void SetAttackPhaseType(Phase phase)
    {
        _phase = phase;
    }
    internal float AttackSelecteTimeStamp = -1f;
    public AttackModule SetCurrentAttack(float dist)
    {
        AttackSelecteTimeStamp = Time.time;

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
            am = _defaultAttack;
            _defaultAttack.RangeType = _attackRangeType; // temp logic default attack do not check IsAttackable() so range is not setted
        }
        
        _currentAttack = am;

        CurrentAttack.StartAction(_owner);

        return am;
    }


    internal AttackModule TryGetPriorityAttack()
    {
        Span<int> availableAttackIndices = stackalloc int[_modules.Length]; // ������ ���ݵ��� �ε����� ������ ����
        int index = 0;

        for (int i = 0; i < _modules.Length; i++)// ������ ���ݵ��� �ε����� ã�� ����
        {
            AttackModule attack = _modules[i];
            if (attack.IsAttackable(_attackRangeType, _phase))
            {
                availableAttackIndices[index++] = i;
            }
        }
        if (index == 0) // ������ ������ ������ null ��ȯ
        {
            return null;
        }

        Sort.QuickSort(availableAttackIndices.Slice(0, index), (l, r) => // �켱������ ���� ����
        {
            return _modules[l].AttackModuleData.Priority.CompareTo(_modules[r].AttackModuleData.Priority);
        });
        Span<int> samePriorityIndices = stackalloc int[index]; // �켱������ ���� ���ݵ��� �ε����� ������ ����
        int samePriorityIndex = 0;
        int p = _modules[availableAttackIndices[0]].AttackModuleData.Priority; // ù��° ������ �켱����
        for (int i = 0; i < index; i++) // �켱������ ���� ���ݵ��� �ε����� ã�� ����
        {
            if (_modules[availableAttackIndices[i]].AttackModuleData.Priority == p)
            {
                samePriorityIndices[samePriorityIndex++] = availableAttackIndices[i];
            }
        }

        _currentAttack = _modules[samePriorityIndices[UnityEngine.Random.Range(0, samePriorityIndex)]]; 
        // �켱������ ���� ���ݵ� �� �������� �ϳ� ����
        return _currentAttack;
    }
    internal AttackModule TryGetRandomAvailableAttack(float dist)
    {
        Span<int> availableAttackIndices = stackalloc int[_modules.Length];
        int index = 0;

        for (int i = 0; i < _modules.Length; i++)
        {
            AttackModule attack = _modules[i];
            if (attack == null) break;
            if (attack.IsAttackable(_attackRangeType, _phase) && dist < attack.AttackModuleData.AttackRange)
            {
                availableAttackIndices[index++] = i;
            }
        }

        if (index == 0)
        {
            return _defaultAttack;
        }

        int selectedIndex = availableAttackIndices[UnityEngine.Random.Range(0, index)];
        _currentAttack = _modules[selectedIndex];
        return _currentAttack;
    }

    public void StartAttackAnim()
    {

        if (CurrentAttack.AttackModuleData.IsAttackType == false)
        {
            return;
        }

        CurrentAttack.AttackModuleData.StartAnim(_owner);

        _isAnimAttacking = true;
        _animator.SetInteger("AttackId", _currentAttack.AttackModuleData.Id);
        _animator.SetTrigger("Attack");
        //SM.Instance.PlaySound2("NPCAttack", this._firePos.transform.position);
    }
    public void StartModulAttack(int type)
    {
        _currentAttack.StartAttack(_owner, type);
    }
    public void StartAttackMove(int type)
    {
        _currentAttack.StartAttackMove(_owner, type);
        return;
    }
    public void EnableDamageBox(float damage, Vector3 offset, Vector3 range)
    {
        if (_attackCollider == null)
            return;
        _attackCollider.SetRange(range);
        _attackCollider.SetOffset(offset);
        _attackCollider.EnableOnly(damage);
        SM.Instance.PlaySound2("NPCAttack", this._firePos.transform.position);

    }

    internal void OnAnimEnd()
    {
        _isAnimAttacking = false;
    }

    public void OnCollisionStay(Collision collision)
    {
    }


    // internal AttackModule TryGetPriorityAttack_old() // ���� �켱���� ���� ���� ���� �޸� �Ҵ� ������ ����
    // {
    //     AttackModule[] availableAttacks = new AttackModule[_modules.Length]; //������ ������ ��� �迭
    //     int index = 0;

    //     foreach (AttackModule attack in _modules) // ��Ÿ���� �ƴ� ���� ������ ���ݵ��� �迭�� �߰�
    //     {
    //         if (attack.IsAttackable(_attackRangeType, _phase))
    //         {
    //             availableAttacks[index++] = attack;
    //         }
    //     }

    //     if (index == 0) // ������ ������ ������ null ��ȯ
    //     {
    //         return null;
    //     }

    //     Sort.QuickSort(availableAttacks.AsSpan(0, index), (l, r) => // �켱������ ���� ����
    //     {
    //         return l.AttackModuleData.Priority.CompareTo(r.AttackModuleData.Priority);
    //     });
    //     AttackModule[] samePriority = new AttackModule[index]; // �켱������ ���� ���ݵ��� ��� �迭
    //     int p = availableAttacks[0].AttackModuleData.Priority;
    //     int samePriorityIndex = 0;
    //     foreach (AttackModule attack in availableAttacks)
    //     {
    //         if (attack.AttackModuleData.Priority == p)
    //         {
    //             samePriority[samePriorityIndex++] = attack;
    //         }
    //     }

    //     _currentAttack = samePriority[UnityEngine.Random.Range(0, samePriorityIndex)]; 
    //     // �켱������ ���� ���ݵ� �� �������� �ϳ� ����
    //     return _currentAttack;
    // }
    internal AttackModule TryGetPriorityAttack_old() // ���� �켱���� ���� ���� ���� �޸� �Ҵ� ������ ����
    {
        List<AttackModule> availableAttacks = new List<AttackModule>(); //������ ������ ��� ����Ʈ

        foreach (AttackModule attack in _modules) // ��Ÿ���� �ƴ� ���� ������ ���ݵ��� ����Ʈ�� �߰�
        {
            if (attack.IsAttackable(_attackRangeType, _phase))
            {
                availableAttacks.Add(attack);
            }
        }
        if (availableAttacks.Count == 0) // ������ ������ ������ null ��ȯ
        {
            return null;
        }

        // availableAttacks.Sort((l, r) => // �켱������ ���� ����
        // {
        //     return l.AttackModuleData.Priority.CompareTo(r.AttackModuleData.Priority);
        // });

        Sort.QuickSort(availableAttacks.ToArray().AsSpan(), (l, r) => // �켱������ ���� ����
        {
            return l.AttackModuleData.Priority.CompareTo(r.AttackModuleData.Priority);
        });

        List<AttackModule> samePriority = new List<AttackModule>(); // �켱������ ���� ���ݵ��� ��� ����Ʈ
        int p = availableAttacks[0].AttackModuleData.Priority;
        foreach (AttackModule attack in availableAttacks)
        {
            if (attack.AttackModuleData.Priority == p)
            {
                samePriority.Add(attack);
            }
        }

        _currentAttack = samePriority[UnityEngine.Random.Range(0, samePriority.Count)]; 
        // �켱������ ���� ���ݵ� �� �������� �ϳ� ����
        return _currentAttack;
    }

}