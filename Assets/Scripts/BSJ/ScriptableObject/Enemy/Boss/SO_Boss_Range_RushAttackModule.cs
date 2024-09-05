using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Range_RushAttackModuleData", menuName = "Enemy/AttackModule/Boss_Range_RushAttackModule")]
public class SO_Boss_Range_RushAttackModule : SO_AttackModule
{
    public float DashVelocity = 15f;
    public float DashTime = 1f;
    public float HommingForce = 10f;

    public override void StartAttackMove(EnemyBase owner)
    {
        base.StartAttackMove(owner);
        owner.Animator.SetBool("EndAttackMove", false);
        Transform targetTrf = owner.Detector.GetLatestTarget();
        Vector3 start = owner.transform.position;
        Vector3 end = targetTrf.transform.position;

        Vector3 dir = end - start;
        dir = dir.normalized;

        Vector3 vel = dir.normalized * DashVelocity;

        owner.Move.StartHommoingGroundedDash(vel,targetTrf,HommingForce);
    }
    public override void UpdateAttackMove(float deltatime, EnemyBase owner)
    {
        base.UpdateAttackMove(deltatime, owner);

        if(owner.Detector.TargetDistance > 2f)
        {
            Transform targetTrf = owner.Detector.GetLatestTarget();
            Vector3 start = owner.transform.position;
            Vector3 end = targetTrf.transform.position;

            Vector3 dir = end - start;
            dir = dir.normalized;

            Vector3 vel = dir.normalized * DashVelocity;
        }
        else
        {
            owner.Animator.SetTrigger("EndAttackMove");
            owner.Move.ResetDash();
            owner.Attack.CurrentAttack.IsUpdateMove = false;
        }

        if(Time.time > owner.Attack.CurrentAttack.PrevAttackTime + DashTime)
        {
            owner.Animator.SetTrigger("EndAttackMove");
            owner.Move.ResetDash();
            owner.Attack.CurrentAttack.IsUpdateMove = false;
        }
    }

    // called in animation event
    public override void StartAttack(EnemyBase owner)
    {
        owner.Attack.EnableDamageBox();
    }
}
