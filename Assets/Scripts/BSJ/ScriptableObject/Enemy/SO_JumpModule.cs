using EnumTypes;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "JumpingAttackModuleData", menuName = "Enemy/AttackModule/AttackJumping")]
public class SO_JumpModule : SO_AttackModule
{
    public float AimRotateSpeed = 10f;
    public float JumpAngle = 30f;
    public float MeleeRange = 3f;
    public float HommingForce = 100f;
    public float AttackDamage = 100f;
    public float JumpAttackDamage = 100f;
    public float AttackCancleDistance = .7f;
    public override void StartAttackMove(EnemyBase owner)
    {
        EnemyMove move = owner.Move;
        Transform transform = owner.transform;
        Transform targetTrf = owner.Detector.GetLatestTarget();
        Rigidbody rb = owner.Rigidbody;
        Animator animator = owner.Animator;
        GameObject gameObject = owner.gameObject;
        Collider characterCollider = owner.CharacterCollider;

        owner.SetEnableRigidbody(true);

        move.JumpedTime = Time.time;
        move.IsJumping = true;
        move.IsLanded = false;

        Vector3 targetDir = (-transform.position + targetTrf.position).normalized;
        float angleV = Mathf.Atan2(targetDir.y, 1f);
        angleV = Mathf.Rad2Deg * angleV;
        angleV = -angleV + JumpAngle;

        rb.velocity = ProjectileCalc.CalcLaunch(transform.position, targetTrf.position, angleV);
        animator.SetBool("EndAttackMove", false);
        gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
        characterCollider.gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
    }
    public override void UpdateAttackMove(float time, EnemyBase owner)
    {
        EnemyMove move = owner.Move;
        Transform transform = owner.transform;
        Vector3 pos = transform.position;
        Vector3 target = owner.Detector.GetLatestTarget().position;
        Vector3 desiredDir = -pos + target;
        float distance = desiredDir.magnitude;

        desiredDir = new Vector3(desiredDir.x, 0f, desiredDir.z);
        desiredDir = desiredDir.normalized;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredDir), Time.deltaTime * AimRotateSpeed);

        bool isGrounded = Physics.CheckSphere(transform.position + (Vector3.up * .35f), .4f, LayerMask.GetMask("Environment"));

        if (!isGrounded)
        {
            if (owner.Move.IsLanded)
            {
                transform.Translate(-Vector3.up * Time.deltaTime * 10f);
            }
            else
            {
                float distH = PhysicsHelper.Dist2D(pos, target);
                bool isAlmostGrouonded = Physics.CheckSphere(transform.position, 1f, LayerMask.GetMask("Environment"));
                isAlmostGrouonded = isAlmostGrouonded && move.Rigidbody.velocity.y < 0f;
                if (distH < AttackCancleDistance || isAlmostGrouonded)
                {
                    if (owner.Move.IsJumping)
                    {
                        owner.Move.IsJumping = false;
                        owner.Animator.SetTrigger("EndAttackMove");
                    }
                    float distV = PhysicsHelper.Dist2D(pos, target);
                }
                else
                {
                    ProjectileCalc.Homming(move.Rigidbody, target, HommingForce);
                }
            }
        }
        else
        {
            if (Time.time - owner.Move.JumpedTime <= .1f)
            {
                return;
            }
            owner.SetEnableRigidbody(false);
            if (owner.Move.IsJumping)
            {
                owner.Move.IsJumping = false;
                owner.Animator.SetTrigger("EndAttackMove");
            }
        }
    }
}
