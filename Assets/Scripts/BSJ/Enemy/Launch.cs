using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    private float aimRotateSpeed = 10f;

    private bool isInit = false;

    private Detector detector;

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
        Vector3 targetDir = (-transform.position + targetTrf.position).normalized;
        float angleV = Mathf.Atan2(targetDir.y, 1f);
        angleV = Mathf.Rad2Deg * angleV;
        angleV = (angleV > -15f) ? angleV + 30f : -angleV;

        rb.velocity = ProjectileCalc.CalcLaunch(transform.position, targetTrf.position, angleV);
        animator.SetBool(hashEndLaunch, false);
        gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
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

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDir), Time.deltaTime * aimRotateSpeed);

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
                float deltaDist = (curPlayerPos - prevPlayerPos).magnitude;
                Vector3 targetDir = (-rb.position + targetTrf.position).normalized;
                Vector3 targetDirH = targetDir;
                targetDirH.y = 0f;
                targetDirH = targetDirH.normalized;

                Vector3 velocityH = rb.velocity;
                velocityH.y = 0f;
                float velocityHMag = velocityH.magnitude;

                Vector3 newVelocityH = targetDirH * velocityHMag;

                Vector3 result = new Vector3(newVelocityH.x, rb.velocity.y, newVelocityH.z);
                result += targetDirH * deltaDist * .5f;

                rb.velocity = result;
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
}
