using BehaviorDesigner.Runtime;
using EnumTypes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyBase : MonoBehaviour, ITargetable
{
    [SerializeField] protected SO_EnemyBase _enemyData;
    [SerializeField] protected EnemyAttack _enemyAttack;
    [SerializeField] protected EnemyMove _enemyMove;
    [SerializeField] protected Detector _detector;
    [SerializeField] protected Combat _combat;
    public Action<EnemyBase> OnDeadWithSelf;

    public Detector Detector => _detector;
    public EnemyAttack Attack => _enemyAttack;
    public EnemyMove Move => _enemyMove;

    protected Animator _animator;
    protected NavMeshAgent _navMeshAgent;
    protected BehaviorTree _behaviorTree;
    protected Rigidbody _rigidbody;
    protected Collider _characterCollider;
    protected Collider _characterEnvCollider;
    public Animator Animator => _animator;
    public NavMeshAgent NavAgent => _navMeshAgent;
    public Rigidbody Rigidbody => _rigidbody;
    public Collider CharacterCollider => _characterCollider;

    protected AIState _aiState = AIState.Idle;
    protected float _currentStateTime = 0f;
    public float CurrentStateTime => _currentStateTime;

    protected bool _isMovable = true;
    public bool IsMovable => _isMovable;

    protected IObjectPool<GameObject> _pooledHitVfx;
    [SerializeField] protected SO_SKillEvent hitVFX;
    [SerializeField] protected GameObject _pooledHitVfxPrefab;


    [SerializeField] protected Transform _firePos;
    [SerializeField] protected GameObject _projectilePrefab;

    protected int _goldDropAmount;
    private bool _isKnocked;

    protected void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;

        _characterCollider = GetComponentInChildren<Collider>();

        _pooledHitVfx = new ObjectPool<GameObject>(CreatePool, OnGetPool,
            OnReleasePool, OnDestroyPool, true, 100, 200);

        _rigidbody = GetComponent<Rigidbody>();
        _behaviorTree = GetComponent<BehaviorTree>();
        _animator = GetComponent<Animator>();

        _detector.Init(this, "Player",
            _enemyData.EnemyAlramDistance,
            false);

        _combat = new Combat();
        _combat.Init(gameObject, _enemyData.Hp);
        _combat.InvincibleTimeOnHit = 0f;
        _combat.OnDamaged += OnDamaged;
        _combat.OnKnockback += OnKnockback;
        _combat.OnDead += OnDead;

        _enemyAttack.Init(this, GetComponentInChildren<DamageBox>(),
            _enemyData, _animator);
        _enemyMove.Init(transform, _rigidbody);

        _goldDropAmount = _enemyData.DropGoldAmount;

        SharedFloat attackRange = new SharedFloat();
        attackRange.Value = _enemyData.AttackRange;
        attackRange.Value = 0f;
        SharedFloat detectRange = new SharedFloat();
        detectRange.Value = _enemyData.EnemyAlramDistance;
        SharedFloat enemyAlramLimitTime = new SharedFloat();
        enemyAlramLimitTime.Value = 999999f;

        _behaviorTree.SetVariable("AttackRange", attackRange);
        _behaviorTree.SetVariable("DetectRange", detectRange);
        _behaviorTree.SetVariable("EnemyAlramLimitTime", enemyAlramLimitTime);
    }
    protected void OnDestroy()
    {
        _combat.OnDead -= OnDead;
        _combat.OnDamaged -= OnDamaged;
        _combat.OnKnockback -= OnKnockback;
    }
    Quaternion look;
    float rotateSpeed = 10f;
    protected void Update()
    {
        _combat.DoUpdate(Time.deltaTime);
        _enemyAttack.DoUpdate(Time.deltaTime);
        _enemyMove.DoUpdate(Time.deltaTime);
        if (AnimatorHelper.IsOnlyAnimationPlaying(_animator, 0, "Base Layer.Hit"))
        {
            _isKnocked = false;
            return;
        }
        else
        {
            _isKnocked = true;
        }

        if (_aiState == AIState.Dead)
        {
            if (Mathf.Abs(_rigidbody.velocity.y) < .1f)
            {
                _rigidbody.velocity = Vector3.zero;
            }
            return;
        }

        if(Attack.IsAttacking)
        {
            _isMovable = false;
        }
        else
        {
            _isMovable = true;
        }

        if (Move.IsMoving)
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }

        _currentStateTime += Time.deltaTime;

        UpdateRotation();
    }

    protected void UpdateRotation()
    {
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



    public void SetState(AIState state)
    {
        _aiState = state;
        _currentStateTime = 0f;
    }
    public bool IsAttackable()
    {
        return !_enemyAttack.IsAttacking;
    }

    protected void ResetEnemy()
    {
        SetEnableAllCollision(true);
        _animator.SetBool("IsDead", false);
        gameObject.SetActive(true);
    }
    protected void SetEnableAllCollision(bool condition)
    {
        _characterCollider.enabled = condition;

        if (_characterEnvCollider != null)
        {
            _characterEnvCollider.enabled = condition;
        }
    }
    public void SetEnableRigidbody(bool condition)
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

    float _debuff_Passive_Offensive2_IncreasedDamageTakenMulti = 1;
    public void ActiveDebuff_Passive_Offensive2(float value)
    {
        _debuff_Passive_Offensive2_IncreasedDamageTakenMulti = 1 + value;
    }
    public virtual Vector3 GetPosition()
    {
        return transform.position;
    }
    public virtual void Hit(float dmg, DamageType type = DamageType.Normal)
    {
        dmg *= _debuff_Passive_Offensive2_IncreasedDamageTakenMulti;
        _debuff_Passive_Offensive2_IncreasedDamageTakenMulti = 1;
        _combat.Damaged(dmg, type);
        GameObject hitEF = ObjectPoolManager.Instance.DequeueObject(hitVFX.preFab);
        Vector3 finalPosition = this.transform.position + transform.TransformDirection(hitVFX.offSet);
        hitEF.transform.position = finalPosition;
        DmgTextManager.Instance.OnDmged(dmg, this.transform.position);
    }
    public virtual bool IsDead()
    {
        return _combat.IsDead();
    }
    protected void OnDamaged(DamageType damageType)
    {
    }

    protected void OnKnockback()
    {
        _animator.SetTrigger("Hit");
    }


    #region OnDead
    protected void OnDead()
    {
        OnDeadWithSelf.Invoke(this);

        _characterCollider.gameObject.layer = LayerMask.NameToLayer("Ragdoll");

        if (_characterEnvCollider != null)
        {
            _characterEnvCollider.gameObject.layer = LayerMask.NameToLayer("Ragdoll");
        }

        _rigidbody.freezeRotation = true;
        SetEnableRigidbody(true);

        _aiState = AIState.Dead;
        _animator.SetTrigger("Dead");
        _animator.SetBool("IsDead", true);
        _behaviorTree.DisableBehavior();

        PlayerMaster.Instance._PlayerInstanteState.OnEnemyDestroy();

        StopAllCoroutines();
        StartCoroutine(DelayedDisable());

        DropGold();
    }

    protected void DropGold()
    {
        GameManager.Instance._PlayerMaster._PlayerInstanteState.AddGold(_goldDropAmount);
    }

    protected IEnumerator DelayedDisable()
    {
        yield return new WaitForSeconds(6f);
        gameObject.SetActive(false);
    }
    #endregion


    protected void OnCollisionStay(Collision collision)
    {
        Move.OnCollisionStay(collision);
        Attack.OnCollisionStay(collision);
    }

    #region Nav
    public void Idle()
    {
        _navMeshAgent.isStopped = true;
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

    #endregion

    #region DebugEnemy

    protected bool _debug = false;

    public void EnableDebug()
    {
        _debug = true;
    }
    protected void EnemyDebug()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_detector.transform.position, _enemyData.AttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_detector.transform.position, _enemyData.EnemyAlramDistance);

        Gizmos.color = GetColorByState(_aiState);
        Gizmos.DrawSphere(transform.position + Vector3.up * 2.5f, .5f);
    }

    protected void OnDrawGizmosSelected()
    {
        EnemyDebug();
    }

    protected void OnDrawGizmos()
    {
        if (_debug)
        {
            EnemyDebug();
            _debug = false;
        }
    }

    protected Color GetColorByState(AIState state)
    {
        if (_enemyAttack.IsAttacking)
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


    #region VFXPool

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
    protected IEnumerator DelayedRealease(GameObject vfx)
    {
        yield return new WaitForSeconds(2f);
        _pooledHitVfx.Release(vfx);
    }
    #endregion
}
