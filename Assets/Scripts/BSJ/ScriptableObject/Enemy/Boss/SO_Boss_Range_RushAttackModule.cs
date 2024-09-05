using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Range_RushAttackModuleData", menuName = "Enemy/AttackModule/Boss_Range_RushAttackModule")]
public class SO_Boss_Range_RushAttackModule : SO_AttackModule
{
    public float DashVelocity = 15f;
    public float DashTime = 1f;
    public override void StartAttack(EnemyBase owner)
    {
        owner.Animator.SetBool("EndAttackMove", false);
        owner.gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
        owner.CharacterCollider.gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
    }
    public override void UpdateAttack(float deltatime, EnemyBase owner)
    {
        base.UpdateAttack(deltatime, owner);
        Transform transform = owner.transform;

        Vector3 vel = (transform.forward * DashVelocity);
        EnemyMove move = owner.Move;

        if (!move.IsGrounded)
        {
            transform.Translate(-Vector3.up * deltatime * 10f);
        }
        else
        {
            owner.Rigidbody.velocity = vel;
        }
        if(Time.time > owner.Attack.CurrentAttack.PrevAttackTime + DashTime)
        {
            owner.Animator.SetTrigger("EndAttackMove");
            owner.gameObject.layer = LayerMask.NameToLayer("EnemyCollider");
            owner.CharacterCollider.gameObject.layer = LayerMask.NameToLayer("LaunchedEnemy");
        }
    }
}
