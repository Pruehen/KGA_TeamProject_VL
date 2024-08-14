using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class Launch : AiAttackAction
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
    private float attackDistance;
    private Vector3 prevPlayerPos;
    Vector3 SentinelVec = new Vector3(-9999f, -9999f, -9999f);

    private bool isInit = false;

    private Detector detector;

    private float initialDistance;
    [SerializeField] private float _aimRotateSpeed = 10f;
    [SerializeField] private float _hommingPower = 100f;
    [SerializeField] private float _meleeRange = 3f;

    public Launch(MonoBehaviour owner, Detector detector)
    {
        this.owner = owner;
        gameObject = owner.gameObject;
        transform = gameObject.transform;
        this.detector = detector;
        rb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        prevPlayerPos = SentinelVec;


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
    public void DoAttack()
    {
        owner.StartCoroutine(ResetLaunching());
        isInit = false;

        rb.isKinematic = true;
        animator.SetBool(hashEndLaunch, false);
        Vector3 enemyToPlayerDir = (-transform.position + targetTrf.position).normalized;
        gameObject.layer = LayerMask.NameToLayer("EnemyCollider");
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
        targetTrf = detector.GetTarget();
        initialDistance = (transform.position - targetTrf.position).magnitude;
        Vector3 targetDir = (-transform.position + targetTrf.position).normalized;
        float angleV = Mathf.Atan2(targetDir.y, 1f);
        angleV = Mathf.Rad2Deg * angleV;
        angleV = (angleV > -15f) ? angleV + 30f : -angleV;

        rb.velocity = ProjectileCalc.CalcLaunch(transform.position, targetTrf.position, angleV);
        animator.SetBool(hashEndLaunch, false);
        gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
    }

    private IEnumerator ResetLaunching()
    {
        yield return new WaitForSeconds(1f);
        agent.nextPosition = transform.position;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.isStopped = false;
        prevPlayerPos = SentinelVec;
        isInit = false;
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

        bool isGrounded = Physics.CheckSphere(transform.position, .2f, LayerMask.GetMask("Environment"));

        if (!isGrounded)
        {
            Vector3 curPlayerPos = targetTrf.position;
            // 초기값
            if (prevPlayerPos == SentinelVec)
                prevPlayerPos = curPlayerPos;


            bool isAlmostGrouonded = Physics.CheckSphere(transform.position, .5f, LayerMask.GetMask("Environment"));
            isAlmostGrouonded = isAlmostGrouonded && rb.velocity.y < 0f;
            if (distance < attackDistance || isAlmostGrouonded)
            {
                animator.SetTrigger(hashEndLaunch);
            }
            else
            {
                ProjectileCalc.Homming(rb,targetTrf,_hommingPower);
            }
            prevPlayerPos = curPlayerPos;
        }
        else
        {
            if(rb.velocity.y <=-.1f)
            {
                prevPlayerPos = SentinelVec;
                animator.SetBool(hashEndLaunch, true);
            }
        }
    }
    public bool IsAttacking()
    {
        return isInit;
    }

    public void StartAttackAnim()
    {
        if(Vector3.Distance(detector.GetPosition(),transform.position) <= _meleeRange)
        {
            agent.nextPosition = transform.position;
            animator.SetBool("IsLaunch", false);
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetBool("IsLaunch", true);
        }
    }
}
