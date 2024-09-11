using UnityEngine;

[CreateAssetMenu(fileName = "RangeAttackModuleData", menuName = "Enemy/AttackModule/AttackRange")]
public class SO_RangeModule : SO_AttackModule
{
    public GameObject Prefab_projectile;
    public float ProjectileDamage;
    public float ProjectileSpeed;

    public override void StartAttack(EnemyBase owner, int type)
    {
        base.StartAttack(owner, type);
        Transform targetTrf = owner.Detector.GetLatestTarget();
        Transform firePos = owner.FirePos;

        ShootProjectile(targetTrf, firePos);
    }

    protected void ShootProjectile(Transform targetTrf, Transform firePos)
    {
        Vector3 vel = ProjectileCalc.CalculateInitialVelocity(targetTrf
            , firePos, ProjectileSpeed, Vector3.up * 1f);
        GameObject projectileObject = ObjectPoolManager.Instance.DequeueObject(Prefab_projectile);
        projectileObject.transform.position = firePos.position;
        projectileObject.transform.rotation = firePos.rotation;
        EnemyProjectile projectile = null;
        projectileObject.TryGetComponent(out projectile);

        projectile.Fire(vel, ProjectileDamage);
    }
}
