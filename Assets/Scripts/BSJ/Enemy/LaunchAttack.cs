using EnumTypes;
using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class LaunchAttack
{
    //private Transform transform;
    //private Transform targetTrf;
    //private Rigidbody rb;
    //private Animator animator;
    //private NavMeshAgent agent;
    //private float nearDistance = .7f;
    //private Collider characterCollider;
    //private bool isInit = false;
    //private float initialDistance;
    //bool _isCrashed = false;
    //float _jumpTimeStamp = 0f;
    //public void DoUpdate()
    //{
    //    UpdateInLaunch();
    //}


    //private void OnDestroy()
    //{
    //    owner = null;
    //}

    ///// <summary>
    ///// ¿Ã∞Õ
    ///// </summary>
    //public void DoAttack(DamageBox damageBox, AttackType enemyAttackType)
    //{
    //    switch (enemyAttackType)
    //    {
    //        case AttackType.Melee:
    //            damageBox.EnableDamageBox(_attackDamage);
    //            break;
    //        case AttackType.Jump:
    //            damageBox.EnableDamageBox(_jumpAttackDamage);
    //            break;
    //    }
    //}
    //private void DisablePhysics()
    //{
    //    isInit = false;
    //    agent.nextPosition = transform.position;
    //    animator.SetBool(hashEndLaunch, false);
    //    gameObject.layer = LayerMask.NameToLayer("EnemyCollider");
    //    characterCollider.gameObject.layer = LayerMask.NameToLayer("EnemyCollider");
    //    rb.isKinematic = true;
    //}


    //public void StartAttackAnim()
    //{
    //    targetTrf = detector.GetTarget();
    //    if (Vector3.Distance(detector.GetPosition(), transform.position) <= _meleeRange)
    //    {
    //        agent.nextPosition = transform.position;
    //        animator.SetBool("IsLaunch", false);
    //        animator.SetTrigger("Attack");
    //    }
    //    else
    //    {
    //        animator.SetBool("IsLaunch", true);
    //        animator.SetTrigger("Attack");
    //    }
    //}
    //private bool isJumping;

    //public void OnExcuteLaunch()
    //{
    //    _jumpTimeStamp = Time.time;
    //    _isCrashed = false;
    //    isJumping = true;

    //    agent.nextPosition = transform.position;
    //    agent.updatePosition = false;
    //    agent.updateRotation = false;
    //    agent.isStopped = true;
    //    isInit = true;
    //    rb.isKinematic = false;
    //    initialDistance = (transform.position - targetTrf.position).magnitude;
    //    Vector3 targetDir = (-transform.position + targetTrf.position).normalized;
    //    float angleV = Mathf.Atan2(targetDir.y, 1f);
    //    angleV = Mathf.Rad2Deg * angleV;
    //    angleV = -angleV + _jumpAngle;

    //    rb.velocity = ProjectileCalc.CalcLaunch(transform.position, targetTrf.position, angleV);
    //    animator.SetBool(hashEndLaunch, false);
    //    gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
    //    characterCollider.gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
    //}

    //private void ResetLaunching()
    //{
    //    DisablePhysics();
    //    agent.nextPosition = transform.position;
    //    agent.updatePosition = true;
    //    agent.updateRotation = true;
    //    agent.isStopped = false;
    //}
    //void UpdateInLaunch()
    //{
    //    if (!isInit)
    //    {
    //        return;
    //    }
    //    Vector3 pos = transform.position;
    //    Vector3 target = targetTrf.position;
    //    Vector3 desiredDir = -pos + target;
    //    float distance = desiredDir.magnitude;

    //    desiredDir = new Vector3(desiredDir.x, 0f, desiredDir.z);
    //    desiredDir = desiredDir.normalized;

    //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDir), Time.deltaTime * _aimRotateSpeed);

    //    bool isGrounded = Physics.CheckSphere(transform.position + (Vector3.up * .35f), .4f, LayerMask.GetMask("Environment"));

    //    if (!isGrounded)
    //    {
    //        if (_isCrashed)
    //        {
    //            transform.Translate(-Vector3.up * Time.deltaTime * 10f);
    //        }
    //        else
    //        {
    //            float distH = PhysicsHelper.Dist2D(pos, target);
    //            bool isAlmostGrouonded = Physics.CheckSphere(transform.position, 1f, LayerMask.GetMask("Environment"));
    //            isAlmostGrouonded = isAlmostGrouonded && rb.velocity.y < 0f;
    //            if (distH < nearDistance || isAlmostGrouonded)
    //            {
    //                if (isJumping)
    //                {
    //                    isJumping = false;
    //                    animator.SetTrigger(hashEndLaunch);
    //                }
    //                float distV = PhysicsHelper.Dist2D(pos, target);
    //            }
    //            else
    //            {
    //                ProjectileCalc.Homming(rb, targetTrf, _hommingForce);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (Time.time - _jumpTimeStamp <= .1f)
    //        {
    //            return;
    //        }
    //        DisablePhysics();
    //        ResetLaunching();
    //        if (isJumping)
    //        {
    //            isJumping = false;
    //            animator.SetTrigger(hashEndLaunch);
    //        }
    //        isJumping = false;
    //    }
    //}
    //public bool IsAttacking()
    //{
    //    return isInit;
    //}



    //bool wait = false;
    //public void TriggerOnEnterCollider()
    //{
    //    if (Time.time - _jumpTimeStamp <= .1f)
    //    { return; }
    //    _isCrashed = true;
    //    rb.isKinematic = true;
    //}
}
