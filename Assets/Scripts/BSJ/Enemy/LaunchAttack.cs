using EnumTypes;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class LaunchAttack : AiAttackAction
{
    private MonoBehaviour owner;
    private GameObject gameObject;
    private Transform transform;
    private Transform targetTrf;
    private Rigidbody rb;
    private Animator animator;
    private NavMeshAgent agent;
    private int hashEndLaunch;
    private int hashAttack;
    private float nearDistance = .7f;
    private Vector3 prevPlayerPos;
    Vector3 SentinelVec = new Vector3(-9999f, -9999f, -9999f);

    private bool isInit = false;

    private Detector detector;

    private float initialDistance;
    [SerializeField] private float _aimRotateSpeed = 10f;
    [SerializeField] private float _jumpAngle = 100f;
    [SerializeField] private float _meleeRange = 3f;
    [SerializeField] private float _hommingForce = 100f;
    [SerializeField] private float _attackDamage = 100f;
    [SerializeField] private float _jumpAttackDamage = 100f;

    public LaunchAttack(MonoBehaviour owner, Detector detector, SO_JumpingEnemy enemyData)
    {
        this.owner = owner;
        gameObject = owner.gameObject;
        transform = gameObject.transform;
        this.detector = detector;
        rb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        prevPlayerPos = SentinelVec;
        _meleeRange = enemyData.MeleeRange;
        _jumpAngle = enemyData.JumpAngle;
        _hommingForce = enemyData.HommingForce;
        _attackDamage = enemyData.AttackDamage;
        _jumpAttackDamage = enemyData.JumAttackDamage;

        hashEndLaunch = Animator.StringToHash("EndLaunch");
        hashAttack = Animator.StringToHash("Attack");
    }

    public void DoUpdate()
    {
        UpdateInLaunch();
    }


    private void OnDestroy()
    {
        owner = null;
    }

    /// <summary>
    /// 이것
    /// </summary>
    public void DoAttack(DamageBox damageBox, EnemyAttackType enemyAttackType)
    {
        switch (enemyAttackType)
        {
            case EnemyAttackType.Melee:
                damageBox.EnableDamageBox(_attackDamage);
                break;
            case EnemyAttackType.Jump:
                damageBox.EnableDamageBox(_jumpAttackDamage);
                break;
        }
    }
    private void DisablePhysics()
    {
        isInit = false;
        agent.nextPosition = transform.position;
        animator.SetBool(hashEndLaunch, false);
        gameObject.layer = LayerMask.NameToLayer("EnemyCollider");
        rb.isKinematic = true;
    }
    public void OnExcuteLaunch()
    {
        agent.nextPosition = transform.position;
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.isStopped = true;
        prevPlayerPos = SentinelVec;
        isInit = true;
        rb.isKinematic = false;
        initialDistance = (transform.position - targetTrf.position).magnitude;
        Vector3 targetDir = (-transform.position + targetTrf.position).normalized;
        float angleV = Mathf.Atan2(targetDir.y, 1f);
        angleV = Mathf.Rad2Deg * angleV;
        angleV = -angleV + _jumpAngle;

        rb.velocity = ProjectileCalc.CalcLaunch(transform.position, targetTrf.position, angleV);
        animator.SetBool(hashEndLaunch, false);
        gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
    }

    private void ResetLaunching()
    {
        DisablePhysics();
        agent.nextPosition = transform.position;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.isStopped = false;
        prevPlayerPos = SentinelVec;
    }
    void UpdateInLaunch()
    {
        if (!isInit)
        {
            return;
        }
        Vector3 pos = transform.position;
        Vector3 target = targetTrf.position;
        Vector3 desiredDir = -pos + target;
        float distance = desiredDir.magnitude;

        desiredDir = new Vector3(desiredDir.x, 0f, desiredDir.z);
        desiredDir = desiredDir.normalized;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDir), Time.deltaTime * _aimRotateSpeed);

        bool isGrounded = Physics.CheckSphere(transform.position, .1f, LayerMask.GetMask("Environment"));

        if (!isGrounded)
        {
            Vector3 curPlayerPos = targetTrf.position;
            // 초기값
            if (prevPlayerPos == SentinelVec)
                prevPlayerPos = curPlayerPos;


            float distH = PhysicsHelper.Dist2D(pos, target);
            bool isAlmostGrouonded = Physics.CheckSphere(transform.position, 1f, LayerMask.GetMask("Environment"));
            isAlmostGrouonded = isAlmostGrouonded && rb.velocity.y < 0f;
            if (distH < nearDistance || isAlmostGrouonded)
            {
                animator.SetTrigger(hashEndLaunch);
                float distV = PhysicsHelper.Dist2D(pos, target);
                if (distH < 1.5f && distV < 1.5f)
                {
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                }
            }
            else
            {
                ProjectileCalc.Homming(rb, targetTrf, _hommingForce);
            }
            prevPlayerPos = curPlayerPos;
        }
        else
        {
            if (rb.velocity.y <= -.1f)
            {
                DisablePhysics();
                ResetLaunching();
                prevPlayerPos = SentinelVec;
                animator.SetTrigger(hashEndLaunch);
            }
        }
    }
    public bool IsAttacking()
    {
        return isInit;
    }

    public void StartAttackAnim()
    {
        targetTrf = detector.GetTarget();
        if (Vector3.Distance(detector.GetPosition(), transform.position) <= _meleeRange)
        {
            agent.nextPosition = transform.position;
            animator.SetBool("IsLaunch", false);
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetBool("IsLaunch", true);
            animator.SetTrigger("Attack");
        }
    }
}
