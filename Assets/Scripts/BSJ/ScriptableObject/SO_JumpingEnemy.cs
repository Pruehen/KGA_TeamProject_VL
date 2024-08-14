using UnityEngine;

[CreateAssetMenu(fileName = "JumpingEnemyData", menuName = "Enemy/JumpingEnemy", order = 1)]
public class SO_JumpingEnemy : SO_EnemyBase
{
    [Space(10)]
    [Header("특수 점프")]
    public float MeleeRange = 2f;
    public float JumpAngle = 20f;
    public float HommingForce = 100f;
    public float JumpAttackDamage = 6f;
}
