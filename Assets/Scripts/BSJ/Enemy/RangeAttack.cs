using EnumTypes;
using System;
using UnityEngine;

[Serializable]
public class RangeAttack : AiAttackAction
{
    private MonoBehaviour owner;
    private GameObject gameObject;
    private Transform transform;
    private Transform targetTrf;
    private Animator animator;
    private int hashAttack;

    private Detector detector;

    private float initialDistance;
    [SerializeField] private Transform _firePos;
    [SerializeField] private GameObject Prefab_projectile;
    [SerializeField] private float _aimRotateSpeed = 10f;
    [SerializeField] private float _projectileSpeed = 45f;
    [SerializeField] private float _projectileDamage = 3f;

    private bool rotatable;

    public RangeAttack(MonoBehaviour owner, Detector detector, Transform firePos, SO_RangeEnemy enemyData)
    {
        this.owner = owner;
        gameObject = owner.gameObject;
        transform = gameObject.transform;
        this.detector = detector;
        animator = gameObject.GetComponent<Animator>();

        hashAttack = Animator.StringToHash("Attack");

        Prefab_projectile = enemyData.ProjectilePrefab;
        _firePos = firePos;
        _projectileDamage = enemyData.AttackDamage;
        _projectileSpeed = enemyData.ProjectileSpeed;
    }
    float rotateSpeed = 10f;
    Quaternion look;
    public void DoUpdate()
    {
        if(rotatable)
        {
            if (detector.GetLatestTarget() != null)
            {
                Vector3 orig = transform.position;
                Vector3 target = detector.GetLatestTarget().position;
                orig.y = 0;
                target.y = 0;
                look = Quaternion.LookRotation(target - orig, Vector3.up);
                Rotator.SmoothRotate(transform ,look, rotateSpeed, Time.deltaTime);
            }
        }
        return;
    }
    public void DoAttack(DamageBox damageBox, AttackType enemyAttackType)
    {
        rotatable = false;
        Vector3 enemyToPlayerDir = (-transform.position + targetTrf.position).normalized;
        Vector3 vel = ProjectileCalc.CalculateInitialVelocity(targetTrf
            , _firePos, _projectileSpeed, Vector3.up * 1f);
        GameObject projectileObject = ObjectPoolManager.Instance.DequeueObject(Prefab_projectile);
        projectileObject.transform.position = _firePos.position;
        projectileObject.transform.rotation = _firePos.rotation;
        EnemyProjectile projectile = null;
        projectileObject.TryGetComponent(out projectile);

        projectile.Fire(vel,_projectileDamage);

    }
    public bool IsAttacking()
    {
        return false;
    }
    public void StartAttackAnim()
    {
        rotatable = true;
        targetTrf = detector.GetTarget();
        animator.SetTrigger(hashAttack);
    }
}
