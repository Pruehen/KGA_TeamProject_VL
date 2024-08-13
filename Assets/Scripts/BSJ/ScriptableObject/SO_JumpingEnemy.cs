using UnityEngine;

[CreateAssetMenu(fileName = "JumpingEnemyData", menuName = "Enemy/JumpingEnemy", order = 1)]
public class SO_JumpingEnemy : SO_EnemyBase
{
    [Space(10)]
    [Header("Ư�� ����")]
    public float JumpForce;
    public float JumpAttackDamage;
}
