using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData",menuName = "Enemy/Enemy", order = 0)]
public class SO_EnemyBase : ScriptableObject
{
    [Space(10)]
    [Header("체력")]
    public float Hp = 100f;
    [Space(10)]
    [Header("감지")]
    public float EnemyAlramDistance = 6f;
    [Space(10)]
    [Header("기본 공격")]
    public float AttackDamage = 2f;
    public float AttackRange = 2f;
    public float AttackCooldown = 2f;
    public float AttackMovableCooldown = 0.6f;
    public float AttackSpeedMultiply = 1f;
}
