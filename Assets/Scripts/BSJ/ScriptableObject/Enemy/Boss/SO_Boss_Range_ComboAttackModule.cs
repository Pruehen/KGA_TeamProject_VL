using UnityEngine;

[CreateAssetMenu(fileName = "Boss_Range_ComboAttackModuleData", menuName = "Enemy/AttackModule/Boss_Range_ComboAttackModule")]
public class SO_Boss_Range_ComboAttackModule : SO_RangeModule
{
    public float Interval;
    public float RotateSpeed;

    public override void StartAttack(EnemyBase owner, int type)
    {
        //base.StartAttack(owner, type);

        owner.Move.AttackRotate = true;
        owner.Move.AttackRotateSpeed = RotateSpeed;
    }
    public override void UpdateAttack(EnemyBase owner, int type, float deltaTime)
    {
        base.UpdateAttack(owner, type, deltaTime);
        if (Time.time > owner.Attack.CurrentAttack.PrevFireTime + Interval)
        {
            Transform targetTrf = owner.Detector.GetLatestTarget();
            Transform firePos = owner.FirePos;
            owner.StartVFX("Bose_Spike_Multi_Hand");
            ComboShootProjectile(firePos);
            owner.Attack.CurrentAttack.PrevFireTime = Time.time;
        }
    }
    protected void ComboShootProjectile(Transform firePos)
    {
        Vector3 vel = firePos.forward * ProjectileSpeed;
        GameObject projectileObject = ObjectPoolManager.Instance.DequeueObject(Prefab_projectile);

        projectileObject.transform.position = firePos.position;
        projectileObject.transform.rotation = firePos.rotation;
        EnemyProjectile projectile = null;
        projectileObject.TryGetComponent(out projectile);

        projectile.Fire(vel, ProjectileDamage);
    }
    public override void EndAttack(EnemyBase owner)
    {
        base.EndAttack(owner);
        owner.Animator.SetTrigger("EndAttackMove");
        owner.Move.AttackRotate = false;
        owner.Move.AttackRotateSpeed = RotateSpeed;
    }
}
