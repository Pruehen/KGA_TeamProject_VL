using UnityEngine;

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

    public override void StartAttack(EnemyBase owner)
    {
        base.StartAttack(owner);

        owner.Animator.SetBool("EndAttackMove", false);
    }
    public override void StartAttackMove(EnemyBase owner)
    {
        EnemyMove move = owner.Move;
        Transform transform = owner.transform;
        Transform targetTrf = owner.Detector.GetLatestTarget();
        Rigidbody rb = owner.Rigidbody;
        Animator animator = owner.Animator;
        GameObject gameObject = owner.gameObject;
        Collider characterCollider = owner.CharacterCollider;

        Vector3 targetDir = (-transform.position + targetTrf.position).normalized;
        float angleV = Mathf.Atan2(targetDir.y, 1f);
        angleV = Mathf.Rad2Deg * angleV;
        angleV = -angleV + JumpAngle;
        Vector3 vel = ProjectileCalc.CalcLaunch(transform.position, targetTrf.position, angleV);

        move.HommoingLaunch(vel, owner.Detector.GetLatestTarget(), HommingForce);
        animator.SetBool("EndAttackMove", false);

        owner.Attack.CurrentAttack.hasAttacked = false;
    }
    public override void UpdateAttackMove(float time, EnemyBase owner)
    {
        EnemyMove move = owner.Move;

        if(owner.Attack.CurrentAttack.hasAttacked)
        {
            return;
        }

        if (move.IsHommingEnd || (Vector3.Distance(owner.Detector.GetLatestTarget().position, owner.GetPosition()) <= 4f))
        {
            if (owner.Animator.GetBool("EndAttackMove") == false)
            {
                owner.Attack.CurrentAttack.hasAttacked = true;
                owner.Animator.SetTrigger("EndAttackMove");
            }
        }
    }
}