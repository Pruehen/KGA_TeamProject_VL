using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

[Serializable]
public class EnemyEditorData
{
    [Space(10)]
    [Header("기본 공격")]
    public float AttackDamage = 2f;
    public float AttackRange = 2f;
    public float AttackCooldown = 2f;
    public float AttackMovableCooldown = 0.6f;
    public bool AttackThroughWall = false;
    [Space(10)]
    [Header("감지")]
    public float EnemyPatrolDistance = 4f;
    public float EnemyPatrolIdleDuration = 1f;
    public float EnemyAlramDistance = 6f;
    public float EnemyAlramLimitTime = 2f;
    public bool DetectThroughWall = false;
    public bool CustomPatrolPoint = false;
    //public float EnemyChaseDistance = 9f;
    [Space(10)]
    [Header("특수 공격")]
    public float ChargeAttackForce = 80f;
    [Space(10)]
    public bool CanFireProjectile = false;
    public Transform ProjectileFirePos;
    public GameObject ProjectilePrefab;
    [Space(10)]
    public bool HardLockOn = false;
    [Space(10)]
    public bool ShieldAttack = false;
    public GameObject Shield;
    public float ShieldEnableDelay = .5f;
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, ITargetable
{

    [Header("AI_Patrol")]
    [SerializeField] private Transform _leftPatrolPoint;
    [SerializeField] private Transform _rightPatrolPoint;

    [SerializeField] private string _enemyId;
    [SerializeField] private bool _isMovable = true;
    [SerializeField] private Combat _combat;


    private DamageBox _attackCollider;

    public bool IsMovable
    {
        get => _isMovable;
        private set
        {
            _navMeshAgent.isStopped = !value;
            _isMovable = value;
        }
    }

    [SerializeField] private Detector _detector;

    [SerializeField] private bool _isChargeAttack = false;

    [SerializeField] private EnemyEditorData _editorData;
    [SerializeField] private Transform _rotateTarget;

    [SerializeField] private GameObject _pooledHitVfxPrefab;

    private static float positionZ = 0;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private BehaviorTree _behaviorTree;
    private float _attackDamage;
    private float _colDamage;
    public event Action OnKnockbackEnd;
    private bool _isFlying = false;

    private AIState _aiState = AIState.Idle;

    private float _currentStateTime = 0f;
    public float CurrentStateTime => _currentStateTime;


    private Collider _characterCollider;
    private Collider _characterEnvCollider;

    private IObjectPool<GameObject> _pooledHitVfx;

    private void Awake()
    {

        _rigidbody = GetComponent<Rigidbody>();
        _behaviorTree = GetComponent<BehaviorTree>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _characterCollider = GetComponentInChildren<Collider>();
        _characterEnvCollider = GetComponentInChildren<Collider>();

        _navMeshAgent.updateRotation = false;
        _detector.Init(this, "Player", 5f, false);

        _attackCollider = GetComponentInChildren<DamageBox>();

        Init();

        _pooledHitVfx = new ObjectPool<GameObject>(CreatePool, OnGetPool, OnReleasePool, OnDestroyPool, true, 100, 200);
    }
    private void Init()
    {
        _combat = new Combat();
        _combat.Init(100f);
        _combat.OnDead += OnDead;


        _attackDamage = _editorData.AttackDamage;
        _attackCooldown = _editorData.AttackCooldown;

        //if (_editorData.CustomPatrolPoint == false)
        //{
        //    float moveRange = _editorData.EnemyPatrolDistance;
        //    _leftPatrolPoint.position = transform.position + moveRange * Vector3.right;
        //    _rightPatrolPoint.position = transform.position - moveRange * Vector3.right;
        //}

        _detector.Init(this, "Player",
            _editorData.EnemyAlramDistance,
            _editorData.DetectThroughWall);
        SharedTransformList targetList = new SharedTransformList();
        targetList.Value = new List<Transform>();
        targetList.Value.Add(_leftPatrolPoint);
        targetList.Value.Add(_rightPatrolPoint);

        SharedFloat attackRange = new SharedFloat();
        attackRange.Value = _editorData.AttackRange;
        SharedFloat detectRange = new SharedFloat();
        detectRange.Value = _editorData.EnemyAlramDistance;
        SharedFloat enemyAlramLimitTime = new SharedFloat();
        enemyAlramLimitTime.Value = _editorData.EnemyAlramLimitTime;
        SharedFloat enemyPatrolIdleDuration = new SharedFloat();
        enemyPatrolIdleDuration.Value = _editorData.EnemyPatrolIdleDuration;
        SharedFloat enemyChaseDistance = new SharedFloat();
        //enemyChaseDistance.Value = _editorData.EnemyChaseDistance;
        enemyChaseDistance.Value = 9999f;

        _behaviorTree.SetVariable("TargetList", targetList);
        _behaviorTree.SetVariable("AttackRange", attackRange);
        _behaviorTree.SetVariable("DetectRange", detectRange);
        _behaviorTree.SetVariable("EnemyAlramLimitTime", enemyAlramLimitTime);
        _behaviorTree.SetVariable("EnemyPatrolIdleDuration", enemyPatrolIdleDuration);
        _behaviorTree.SetVariable("ChaseRange", enemyChaseDistance);

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
        if (_aiState == AIState.Dead)
        { return; }
        _currentStateTime += Time.deltaTime;
        if (_currentAttackTime > 0f)
        {
            _currentAttackTime -= Time.deltaTime;
        }
        Vector3 dir = _navMeshAgent.destination - transform.position;
        dir = dir.normalized;
        if (_currentAttackTime > 0f)
        {
            if (_editorData.HardLockOn)
            {
                look = Quaternion.LookRotation(_detector.GetPosition() - transform.position, Vector3.up);
            }
            SmoothRotate(look, rotateSpeed, Time.deltaTime);
        }
        else if (_detector.GetTarget() != null && _aiState == AIState.Chase)
        {
            if (_isFlying)
            {
                look = Quaternion.LookRotation(_detector.GetPosition() - transform.position, Vector3.up);
                SmoothRotate(look, rotateSpeed, Time.deltaTime);
            }
            else
            {
                Vector3 orig = transform.position;
                Vector3 target = _detector.GetPosition();
                orig.y = 0;
                target.y = 0;
                look = Quaternion.LookRotation(target - orig, Vector3.up);
                SmoothRotate(look, rotateSpeed, Time.deltaTime);
            }
        }
        else
        {
            SmoothRotate(look, rotateSpeed, Time.deltaTime);
        }
        if (_navMeshAgent.velocity.magnitude > 0.1f)
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }
    private void ResetEnemy()
    {
        SetEnableAllCollision(true);
        _animator.SetBool("IsDead", false);

        _isMovable = true;
        gameObject.SetActive(true);
    }

    float _attackCooldown = .8f;
    float _currentAttackTime = 0f;

    public void StartAttackAnimation()
    {
        IsMovable = false;
        _currentAttackTime = _attackCooldown;
        _animator.SetTrigger("Attack");
    }
    public bool IsAttackable()
    {
        if (_currentAttackTime > 0f)
        {
            return false;
        }
        return true;
    }

    public bool CharacterAttack()
    {
        StartCoroutine(AttackEnd(_editorData.AttackMovableCooldown));
        Attack();
        return true;
    }
    private IEnumerator AttackEnd(float delay)
    {
        yield return new WaitForFixedUpdate();
        IsMovable = true;
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
        Transform target = _detector.GetTarget();

        if (target == null)
        {
            return false;
        }
        float dist = Vector3.Distance(_detector.GetPosition(), transform.position);
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

    private void OnDamaged()
    {
        GameObject pooled = _pooledHitVfx.Get();
        pooled.transform.position = transform.position;
        pooled.GetComponent<ParticleSystem>().Play();
    }

    private IEnumerator DelayedRealease(GameObject vfx)
    {
        yield return new WaitForSeconds(2f);
        _pooledHitVfx.Release(vfx);
    }

    private void OnDead()
    {
        SetEnableAllCollision(false);
        _aiState = AIState.Dead;
        _animator.SetTrigger("Dead");
        _animator.SetBool("IsDead", true);
        _isMovable = false;
        _behaviorTree.DisableBehavior();
        _navMeshAgent.isStopped = true;

        StopAllCoroutines();
        StartCoroutine(DelayedDisable());
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

    public bool IsTargetVisible(bool isAttack)
    {
        if (isAttack)
        {
            if (_editorData.AttackThroughWall)
            {
                return true;
            }
            return _detector.IsTargetVisible();
        }
        if (_editorData.DetectThroughWall)
        {
            return true;
        }
        return _detector.IsTargetVisible();
    }

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
        Gizmos.DrawWireSphere(_detector.transform.position, _editorData.AttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_detector.transform.position, _editorData.EnemyAlramDistance);

        Gizmos.color = GetColorByState(_aiState);
        Gizmos.DrawSphere(transform.position + Vector3.up, 1f);
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

    internal void SetState(AIState state)
    {
        _aiState = state;
        _currentStateTime = 0f;
    }

    public Transform GetLastTarget()
    {
        return _detector.GetLastTarget();
    }

    internal Vector3 GetTargetPosition()
    {
        return _detector.GetPosition();
    }

    internal Vector3 GetLastTargetPosition()
    {
        return _detector.GetLastPosition();
    }

    internal void Idle()
    {
        _navMeshAgent.isStopped = true;
    }

    private void SmoothRotate(Quaternion targetRotation, float speed, float deltaTime)
    {
        transform.eulerAngles = Quaternion.Lerp(transform.rotation, targetRotation, (speed) * deltaTime).eulerAngles;
    }

    // interface
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Hit(float dmg)
    {
        _combat.Damaged(dmg);
        DmgTextManager.Instance.OnDmged(dmg, this.transform.position);
    }

    public bool IsDead()
    {
        return _combat.IsDead();
    }

}
