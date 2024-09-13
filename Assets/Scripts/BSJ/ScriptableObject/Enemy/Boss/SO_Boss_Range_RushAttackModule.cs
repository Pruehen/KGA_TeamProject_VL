using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Range_RushAttackModuleData", menuName = "Enemy/AttackModule/Boss_Range_RushAttackModule")]
public class SO_Boss_Range_RushAttackModule : SO_AttackModule
{
    public float DashVelocity = 15f;
    public float DashTime = 1f;
    public float HommingForce = 10f;
    public float RushRotateSpeed = 100f;

    public override void StartAttackMove(EnemyBase owner, int type)
    {
        base.StartAttackMove(owner, type);

        owner.Animator.SetBool("EndAttackMove", false);
        Transform targetTrf = owner.Detector.GetLatestTarget();
        Vector3 start = owner.transform.position;
        Vector3 end = targetTrf.transform.position;

        Vector3 dir = end - start;
        dir = dir.normalized;

        Vector3 vel = dir.normalized * DashVelocity;

        owner.Move.StartHommoingGroundedDash(vel,targetTrf,HommingForce);

        owner.Move.AttackRotate = true;
        owner.Move.AttackRotateSpeed = RushRotateSpeed;
    }
    public override void UpdateAttackMove(EnemyBase owner, int type, float deltaTime)
    {
        base.UpdateAttackMove(owner, type, deltaTime);

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
            owner.Move.AttackRotate = false;
        }

        if(Time.time > owner.Attack.CurrentAttack.PrevMoveTime + DashTime)
        {
            owner.Animator.SetTrigger("EndAttackMove");
            owner.Move.ResetDash();
            owner.Attack.CurrentAttack.IsUpdateMove = false;
            owner.Move.AttackRotate = false;
        }
    }

    public override void StartAttack(EnemyBase owner, int type)
    {
        base.StartAttack(owner, type);
        owner.Attack.EnableDamageBox(Damage, DamageBox.Offset, DamageBox.Range);
    }
}
