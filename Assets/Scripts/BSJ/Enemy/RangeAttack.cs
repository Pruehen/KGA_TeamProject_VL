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
    }

    public void DoUpdate()
    {
        return;
    }
    public void DoAttack()
    {
        Vector3 enemyToPlayerDir = (-transform.position + targetTrf.position).normalized;
        Vector3 vel = ProjectileCalc.CalculateInitialVelocity(targetTrf
            , _firePos, _projectileSpeed, Vector3.up * 1f);
        GameObject projectileObject = GameObject.Instantiate(Prefab_projectile,
            _firePos.position, _firePos.rotation);
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
        targetTrf = detector.GetTarget();
        animator.SetBool("IsRangeAttack", false);
        animator.SetTrigger("Attack");
    }
}
