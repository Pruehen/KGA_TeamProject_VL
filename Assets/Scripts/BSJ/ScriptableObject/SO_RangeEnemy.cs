using UnityEngine;

[CreateAssetMenu(fileName = "RangeEnemyData", menuName = "Enemy/RangeEnemy", order = 2)]
public class SO_RangeEnemy : SO_EnemyBase
{
    [Space(10)]
    [Header("특수 원거리")]
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed = 30f;
}
