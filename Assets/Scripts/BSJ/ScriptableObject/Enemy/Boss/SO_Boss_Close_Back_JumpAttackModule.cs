using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Close_Back_JumpAttackModuleData", menuName = "Enemy/AttackModule/Boss_Close_Back_JumpAttack")]
public class SO_Boss_Close_Back_JumpAttackModule : SO_AttackModule
{
    public float JumpAngle = 30f;
    public float JumpForce = 15f;

    public GameObject Prefab_projectile;

    public override void StartAttackMove(EnemyBase owner, int type)
    {
        base.StartAttackMove(owner, type);
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

        owner.Attack.CurrentAttack.hasAttacked = false;
    }
    public override void UpdateAttackMove(EnemyBase owner, int type, float deltaTime)
    {
        base.UpdateAttackMove(owner, type, deltaTime);
        EnemyMove move = owner.Move;

        if (owner.Attack.CurrentAttack.hasAttacked)
        {
            return;
        }

        if (move.IsLanded || move.IsCrashed)
        {
            owner.Attack.CurrentAttack.hasAttacked = true;
            owner.Animator.SetTrigger("EndAttackMove");
        }
    }

    public override void StartAttack(EnemyBase owner, int type)
    {
        base.StartAttack(owner, type);

        Transform targetTrf = owner.Detector.GetLatestTarget();
        Vector3 targetPos = targetTrf.position;
        targetPos.y = 0f;
        
        GameObject projectileObject = ObjectPoolManager.Instance.DequeueObject(Prefab_projectile);
        projectileObject.transform.position = targetPos;
        projectileObject.transform.rotation = Quaternion.identity;
    }
}
