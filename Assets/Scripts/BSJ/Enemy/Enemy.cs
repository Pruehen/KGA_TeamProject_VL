using BehaviorDesigner.Runtime;
using EnumTypes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Pool;

public enum AIState
{
    Idle,
    Patrol,
    Wait,
    Chase,
    JustLostTarget,
    Attack,
    Dead
}
public enum EnemyType
{
    Normal,
    Jump,
    Range
}

public enum DamageType
{
    Normal,
    NonKnockback,
}

public interface AiAttackAction
{
    public void DoAttack(DamageBox damageBox, EnemyAttackType enemyAttackType);
    public void DoUpdate();
    public bool IsAttacking();
    public void StartAttackAnim();
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, ITargetable
{
    [Header("If this var is empty it will spawn _enemyDatas enemy (Not random)")]
    [SerializeField] private SO_RandomEnemySet _randomEnemyData;
    [SerializeField] private SO_EnemyBase _enemyData;
    private EnemyType _enemyType;
    [SerializeField] private bool _isMovable = true;
    [SerializeField] private Combat _combat;
    [SerializeField] SO_SKillEvent hitVFX;

    private DamageBox _attackCollider;

    public bool IsMovable
    {
        get
        {
            return _currentMoveTime <= 0f;
        }
        private set
        {
            _navMeshAgent.isStopped = !value;
            _isMovable = value;
        }
    }

    [SerializeField] private Detector _detector;

    [SerializeField] private Transform _rotateTarget;

    [SerializeField] private GameObject _pooledHitVfxPrefab;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private BehaviorTree _behaviorTree;
    private float _attackDamage;
    private float _colDamage;

    private AIState _aiState = AIState.Idle;

    private float _currentStateTime = 0f;
    public float CurrentStateTime => _currentStateTime;


    private Collider _characterCollider;
    private Collider _characterEnvCollider;

    private IObjectPool<GameObject> _pooledHitVfx;

    private AiAttackAction AiAttack = null;

    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePos;

    private int _goldDropAmount;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _behaviorTree = GetComponent<BehaviorTree>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _characterCollider = GetComponentInChildren<Collider>();

        _navMeshAgent.updateRotation = false;
        _detector.Init(this, "Player", 5f, false);

        _attackCollider = GetComponentInChildren<DamageBox>();

        Init();

        _pooledHitVfx = new ObjectPool<GameObject>(CreatePool, OnGetPool, OnReleasePool, OnDestroyPool, true, 100, 200);
    }

    private void Init()
    {
        if (_randomEnemyData != null)
        {
            SO_EnemyBase[] randomEnemy = _randomEnemyData.RandomEnemySet;
            _enemyData = randomEnemy[UnityEngine.Random.Range(0, randomEnemy.Length)];
        }


        _combat = new Combat();
        _combat.Init(gameObject, _enemyData.Hp);
        _combat.InvincibleTimeOnHit = 0f;
        _combat.OnDamaged += OnDamaged;
        _combat.OnKnockback += OnKnockback;
        _combat.OnDead += OnDead;


        _attackDamage = _enemyData.AttackDamage;
        _attackCooldown = _enemyData.AttackCooldown;
        _attackMoveCooldoown = _enemyData.AttackMovableCooldown;
        AttackSpeedMulti = _enemyData.AttackSpeedMultiply;

        _goldDropAmount = _enemyData.DropGoldAmount;

        _detector.Init(this, "Player",
            _enemyData.EnemyAlramDistance,
            false);

        if (_enemyData is SO_JumpingEnemy)
        {
            _enemyType = EnemyType.Jump;
            AiAttack = new LaunchAttack(this, _detector, _enemyData as SO_JumpingEnemy);
        }
        else if (_enemyData is SO_RangeEnemy)
        {
            _enemyType = EnemyType.Range;
            AiAttack = new RangeAttack(this, _detector, _firePos, _enemyData as SO_RangeEnemy);
        }
        else if (_enemyData is SO_EnemyBase)
        {
            _enemyType = EnemyType.Normal;
            AiAttack = null;
        }
        else
        {
            Assert.IsFalse(true, "적 데이터가 존재하지 않습니다.");
            AiAttack = null;
        }


        SharedFloat attackRange = new SharedFloat();
        attackRange.Value = _enemyData.AttackRange;
        SharedFloat detectRange = new SharedFloat();
        detectRange.Value = _enemyData.EnemyAlramDistance;
        SharedFloat enemyAlramLimitTime = new SharedFloat();
        enemyAlramLimitTime.Value = 999999f;

        _behaviorTree.SetVariable("AttackRange", attackRange);
        _behaviorTree.SetVariable("DetectRange", detectRange);
        _behaviorTree.SetVariable("EnemyAlramLimitTime", enemyAlramLimitTime);
    }

    private void OnDestroy()
    {
        _combat.OnDead -= OnDead;
        _combat.OnDamaged -= OnDamaged;
        _combat.OnKnockback -= OnKnockback;
    }
    public GameObject CreatePool()
    {
        return Instantiate(_pooledHitVfxPrefab);
    }
    public void OnGetPool(GameObject vfx)
    {
        vfx.SetActive(true);
        StartCoroutine(DelayedRealease(vfx));
    }
    public void OnReleasePool(GameObject vfx)
    {
        vfx.SetActive(false);
    }
    public void OnDestroyPool(GameObject vfx)
    {
        Destroy(gameObject);
    }
    Quaternion look;
    float rotateSpeed = 10f;
    private void Update()
    {
        _combat.DoUpdate();
        if (AnimatorHelper.IsAnimationPlaying(_animator, 0, "Base Layer.Hit"))
        {
            IsMovable = false;
            return;
        }
        else
        {
            IsMovable = true;
        }

        if (_aiState == AIState.Dead)
        { return; }

        if (_navMeshAgent.velocity.magnitude > 0.1f)
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }

        if (AiAttack != null)
        {
            AiAttack.DoUpdate();
            if (AiAttack.IsAttacking())
            {
                return;
            }
        }

        _currentStateTime += Time.deltaTime;
        if (_currentAttackTime > 0f)
        {
            _currentAttackTime -= Time.deltaTime;
        }
        if (_currentMoveTime > 0f)
        {
            _currentMoveTime -= Time.deltaTime;
            IsMovable = false;
            return;
        }
        else
        {
            IsMovable = true;
        }
        Vector3 dir = _navMeshAgent.destination - transform.position;
        dir = dir.normalized;
        if (_detector.GetLatestTarget() != null && _aiState == AIState.Chase)
        {
            Vector3 orig = transform.position;
            Vector3 target = _detector.GetLatestTarget().position;
            orig.y = 0;
            target.y = 0;
            look = Quaternion.LookRotation(target - orig, Vector3.up).normalized;
            Rotator.SmoothRotate(transform, look, rotateSpeed, Time.deltaTime);
        }
        else
        {
            Rotator.SmoothRotate(transform, look, rotateSpeed, Time.deltaTime);
        }


    }
    private void ResetEnemy()
    {
        SetEnableAllCollision(true);
        _animator.SetBool("IsDead", false);
        _currentAttackTime = 0f;
        _currentMoveTime = 0f;
        gameObject.SetActive(true);
    }

    float _attackCooldown = .8f;
    float _attackMoveCooldoown = .8f;
    float _currentAttackTime = 0f;
    float _currentMoveTime = 0f;
    float _attackSpeedMulti = 1f;
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

    public void StartAttackAnimation()
    {
        _currentAttackTime = _attackCooldown;
        _currentMoveTime = _attackMoveCooldoown;
        if (AiAttack == null)
        {
            _animator.SetTrigger("Attack");
            return;
        }
        else
        {
            AiAttack.StartAttackAnim();
            return;
        }
    }
    public bool IsAttackable()
    {
        if (_currentAttackTime > 0f)
        {
            return false;
        }
        return true;
    }

    public bool CharacterAttack(EnemyAttackType enemyAttackType)
    {
        if (AiAttack == null)
        {
            StartCoroutine(AttackEnd(_enemyData.AttackMovableCooldown));
            Attack();
            return true;
        }
        else
        {
            StartCoroutine(AttackEnd(_enemyData.AttackMovableCooldown));
            AiAttack.DoAttack(_attackCollider, enemyAttackType);
            return true;
        }
    }
    private void StartLaunching()
    {
        if (AiAttack is LaunchAttack la)
        {
            la.OnExcuteLaunch();
        }
    }

    private IEnumerator AttackEnd(float delay)
    {
        yield return new WaitForFixedUpdate();
    }
    private void Attack()
    {
        if (_attackCollider == null)
            return;
        _attackCollider.EnableDamageBox(_attackDamage);
    }

    public bool IsTargetNear(float range)
    {
        Transform target = _detector.GetTarget();

        if (target == null)
        {
            return false;
        }
        float dist = Vector3.Distance(_detector.GetPosition(), transform.position);
        if (dist <= range)
        {
            return true;
        }
        return false;
    }
    public bool IsTargetFar(float range)
    {
        Transform target = _detector.GetLatestTarget();

        if (target == null)
        {
            return false;
        }
        float dist = Vector3.Distance(_detector.GetLatestTarget().position, transform.position);
        if (dist >= range)
        {
            return true;
        }
        return false;
    }

    public Transform GetTarget()
    {
        return _detector.GetTarget();
    }

    private void OnDamaged(DamageType damageType)
    {
    }

    private void OnKnockback()
    {
        _animator.SetTrigger("Hit");
    }

    private IEnumerator DelayedRealease(GameObject vfx)
    {
        yield return new WaitForSeconds(2f);
        _pooledHitVfx.Release(vfx);
    }
    public Action<Enemy> OnDeadWithSelf;
    private void OnDead()
    {
        OnDeadWithSelf.Invoke(this);

        SetEnableAllCollision(false);
        _aiState = AIState.Dead;
        _animator.SetTrigger("Dead");
        _animator.SetBool("IsDead", true);
        _currentMoveTime = 9999999999f;
        _behaviorTree.DisableBehavior();
        _navMeshAgent.isStopped = true;

        PlayerMaster.Instance._PlayerInstanteState.OnEnemyDestroy();

        StopAllCoroutines();
        StartCoroutine(DelayedDisable());

        DropGold();
    }

    private void DropGold()
    {
        GameManager.Instance._PlayerMaster._PlayerInstanteState.AddGold(_goldDropAmount);
    }

    private void SetEnableAllCollision(bool condition)
    {
        _characterCollider.enabled = condition;

        if (_characterEnvCollider != null)
        {
            _characterEnvCollider.enabled = condition;
        }
    }
    private IEnumerator DelayedDisable()
    {
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }

    private void SetEnableRigidbody(bool condition)
    {
        _navMeshAgent.velocity = Vector3.zero;
        _rigidbody.isKinematic = !condition;

        if (condition)
        {
            _navMeshAgent.updatePosition = false;
        }
        else
        {
            _navMeshAgent.updatePosition = true;
            _navMeshAgent.nextPosition = transform.position;
        }
    }

    public bool IsTargetVisible()
    {
        return _detector.IsTargetVisible();
    }


    #region DebugEnemy
    private void OnDrawGizmosSelected()
    {
        EnemyDebug();
    }

    private void OnDrawGizmos()
    {
        if (_debug)
        {
            EnemyDebug();
            _debug = false;
        }
    }
    private bool _debug = false;
    public void EnableDebug()
    {
        _debug = true;
    }
    private void EnemyDebug()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_detector.transform.position, _enemyData.AttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_detector.transform.position, _enemyData.EnemyAlramDistance);

        Gizmos.color = GetColorByState(_aiState);
        Gizmos.DrawSphere(transform.position + Vector3.up * 2.5f, .5f);
    }

    private Color GetColorByState(AIState state)
    {
        if (_currentAttackTime > 0f)
        {
            return Color.red;
        }
        switch (state)
        {
            case AIState.Idle:
                return Color.green;
            case AIState.Patrol:
                return Color.blue;
            case AIState.Wait:
                return Color.cyan;
            case AIState.Chase:
                return Color.yellow;
            case AIState.JustLostTarget:
                return Color.gray;
            case AIState.Attack:
                return Color.red;
            case AIState.Dead:
                return Color.black;
            default:
                return Color.white;
        }
    }
    #endregion

    public void SetState(AIState state)
    {
        _aiState = state;
        _currentStateTime = 0f;
    }

    public Transform GetLastTarget()
    {
        return _detector.GetLastTarget();
    }

    public Vector3 GetTargetPosition()
    {
        return _detector.GetPosition();
    }

    public Vector3 GetLastTargetPosition()
    {
        return _detector.GetLastPosition();
    }

    public void Idle()
    {
        _navMeshAgent.isStopped = true;
    }

    // interface
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    float _debuff_Passive_Offensive2_IncreasedDamageTakenMulti = 1;
    public void ActiveDebuff_Passive_Offensive2(float value)
    {
        _debuff_Passive_Offensive2_IncreasedDamageTakenMulti = 1 + value;
    }
    public void Hit(float dmg, DamageType type = DamageType.Normal)
    {
        dmg *= _debuff_Passive_Offensive2_IncreasedDamageTakenMulti;
        _debuff_Passive_Offensive2_IncreasedDamageTakenMulti = 1;
        _combat.Damaged(dmg, type);
        GameObject hitEF = ObjectPoolManager.Instance.DequeueObject(hitVFX.preFab);
        Vector3 finalPosition = this.transform.position + transform.TransformDirection(hitVFX.offSet);
        hitEF.transform.position = finalPosition;
        DmgTextManager.Instance.OnDmged(dmg, this.transform.position);
    }

    public bool IsDead()
    {
        return _combat.IsDead();
    }

    public Vector3 GetTargetPositionAlways()
    {
        if (_detector.GetLatestTarget() == null)
        {
            Debug.Log("Cannot find target");
            return Vector3.zero;
        }
        return _detector.GetLatestTarget().position;
    }
    public Transform GetTargetAlways()
    {
        if (_detector.GetLatestTarget() == null)
        {
            Debug.Log("Cannot find target");
            return null;
        }
        return _detector.GetLatestTarget();
    }

    public void ForceAlram()
    {
        if (!gameObject.activeInHierarchy || !gameObject.activeSelf)
        {
            return;
        }
        _detector._detectionRadius = 9999f;
        _detector._detectThroughWall = true;
        SharedFloat detectRange = new SharedFloat();
        detectRange.Value = 9999f;
        _behaviorTree.SetVariable("DetectRange", detectRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (AiAttack is LaunchAttack la)
        {
            la.TriggerOnEnterCollider();
        }
    }
}
