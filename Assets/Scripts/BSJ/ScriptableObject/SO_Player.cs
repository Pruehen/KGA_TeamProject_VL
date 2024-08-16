using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerStat", order = 0)]
public class SO_Player : ScriptableObject
{
    public float maxHp = 100f;
    public float MaxStamina = 800f;
    public float staminaRecoverySpeed = 100f;
    public float staminaRecoveryDelay = 3f;

    public float MaxskillGauge = 100f;
    public int maxBullets = 50;
    public int maxMeleeBullets = 50;

    public float attackSpeed = 1f;
    public float attackPower = 20f;
    public float skillPower = 20f;

    public float moveSpeed = 6f;
}
