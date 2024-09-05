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

        Vector3 targetDir = (-transform.forward).normalized;
        float angleRad = Mathf.Deg2Rad * JumpAngle;
        float verticalVelocity = Mathf.Sin(angleRad) * JumpForce;
        float horizontalVelocity = Mathf.Cos(angleRad) * JumpForce;

        Vector3 vel = (verticalVelocity * Vector3.up) + (targetDir * horizontalVelocity);

        move.Launch(vel);
        animator.SetBool("EndAttackMove", false);
    }
    public override void UpdateAttackMove(float time, EnemyBase owner)
    {
        EnemyMove move = owner.Move;

        if(move.IsLanded || move.IsCrashed)
        {
            owner.Animator.SetTrigger("EndAttackMove");
        }
    }
}
