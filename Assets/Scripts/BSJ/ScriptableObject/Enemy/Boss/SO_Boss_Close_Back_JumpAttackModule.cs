using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Close_Back_JumpAttackModuleData", menuName = "Enemy/AttackModule/Boss_Close_Back_JumpAttack")]
public class SO_Boss_Close_Back_JumpAttackModule : SO_AttackModule
{
    public float JumpAngle = 30f;
    public float JumpForce = 15f;
    public override void StartAttackMove(EnemyBase owner)
    {
        EnemyMove move = owner.Move;
        Transform transform = owner.transform;
        Rigidbody rb = owner.Rigidbody;
        Animator animator = owner.Animator;
        GameObject gameObject = owner.gameObject;
        Collider characterCollider = owner.CharacterCollider;

        owner.SetEnableRigidbody(true);

        move.JumpedTime = Time.time;
        move.IsJumping = true;
        move.IsLanded = false;

        Vector3 targetDir = (-transform.forward).normalized;
        float angleRad = Mathf.Deg2Rad * JumpAngle;
        float verticalVelocity = Mathf.Sin(angleRad) * JumpForce;
        float horizontalVelocity = Mathf.Cos(angleRad) * JumpForce;

        // Combine vertical and horizontal velocities into the final velocity vector
        Vector3 vel = (verticalVelocity * Vector3.up) + (targetDir * horizontalVelocity);

        rb.velocity = vel;
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

        if (!move.IsGrounded)
        {
            if (move.IsCollideOnJump)
            {
                transform.Translate(-Vector3.up * Time.deltaTime * 10f);
            }
            else
            {
                if (move.isAlmostGrouonded && move.IsJumping)
                {
                    move.IsJumping = false;
                    owner.SetEnableRigidbody(false);
                    owner.Animator.SetTrigger("EndAttackMove");
                }
            }
        }
        else
        {
            if (Time.time - move.JumpedTime <= .1f)
            {
                return;
            }
            if (move.IsJumping)
            {
                move.IsJumping = false;
                owner.SetEnableRigidbody(false);
                owner.Animator.SetTrigger("EndAttackMove");
            }
        }
    }
}
