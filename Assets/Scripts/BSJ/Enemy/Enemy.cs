using BehaviorDesigner.Runtime;
using System.Collections;
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
public enum EnemyType
{
    Normal,
    Jump,
    Range
}

public interface AiAttackAction
{
    public void DoAttack();
    public void DoUpdate();
    public bool IsAttacking();
    public void StartAttackAnim();
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, ITargetable
{
    private EnemyType _enemyType;
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

    [SerializeField] private SO_EnemyBase _enemyData;
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
        _combat = new Combat();
        _combat.Init(_enemyData.Hp);
        _combat.OnDead += OnDead;


        _attackDamage = _enemyData.AttackDamage;
        _attackCooldown = _enemyData.AttackCooldown;

        _detector.Init(this, "Player",
            _enemyData.EnemyAlramDistance,
            _enemyData.DetectThroughWall);

        if (_enemyData is SO_EnemyBase)
        {
            _enemyType = EnemyType.Normal;
            AiAttack = null;
        }
        else if (_enemyData is SO_JumpingEnemy)
        {
            _enemyType = EnemyType.Jump;
            AiAttack = new LaunchAttack(this, _detector);
        }
        else if (_enemyData is SO_RangeEnemy)
        {
            _enemyType = EnemyType.Range;
            AiAttack = new RangeAttack(this, _detector, _firePos, _enemyData as SO_RangeEnemy);
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
        Vector3 dir = _navMeshAgent.destination - transform.position;
        dir = dir.normalized;
        if (_currentAttackTime > 0f)
        {
            SmoothRotate(look, rotateSpeed, Time.deltaTime);
        }
        else if (_detector.GetTarget() != null && _aiState == AIState.Chase)
        {
            Vector3 orig = transform.position;
            Vector3 target = _detector.GetPosition();
            orig.y = 0;
            target.y = 0;
            look = Quaternion.LookRotation(target - orig, Vector3.up);
            SmoothRotate(look, rotateSpeed, Time.deltaTime);
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
        _animator.SetTrigger("Attack");
        if (AiAttack == null)
        {
            _currentAttackTime = _attackCooldown;
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

    public bool CharacterAttack()
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
            AiAttack.DoAttack();
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

    private void SmoothRotate(Quaternion targetRotation, float speed, float deltaTime)
    {
        transform.eulerAngles = Quaternion.Lerp(transform.rotation, targetRotation, Mathf.Min((speed) * deltaTime, 1f)).eulerAngles;
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
