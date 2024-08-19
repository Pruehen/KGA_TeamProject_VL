using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerStat", order = 0)]
public class SO_Player : ScriptableObject
{
    [Header ("½ºÅÈ")]
    public float maxHp = 100f;
    public float MaxStamina = 800f;
    public float staminaRecoverySpeed = 100f;
    public float staminaRecoveryDelay = 3f;

    public float attackPower = 20f;
    public float skillPower = 20f;
    public float moveSpeed = 6f;
    public float attackSpeed = 1f;

    public float shieldMax = 100f;
    public float dashTime = .5f;
    public float dashForce = 3f;
    public float dashCost = 300f;

    public float MaxskillGauge = 100f;

    public float statGaugeGainRanged1 = 30f;
    public float statGaugeGainRanged3 = 30f;
    public float statGaugeGainMelee1 = 20f;
    public float statGaugeGainRanged2 = 40f;
    public float statGaugeGainMelee2 = 60f;
    public float statGaugeGainMelee3 = 20f;

    public int maxBullets = 50;
    public int maxMeleeBullets = 50;

    [Space (10)]
    [Header ("°ø°Ý±â")]

    public float atkRanged101 = 1.0f;
    public float atkRanged102 = 2.0f;
    public float atkRanged103 = 0.5f;
    public float atkRanged111 = 2.5f;
    public float atkRanged112 = 5.0f;
    public float atkRanged113 = 1.25f;
    public float atkMelee101 = 5.0f;
    public float atkMelee111 = 7.5f;
    public float atkMelee121 = 2.5f;

    [Header("È¸Àü °ü·Ã")]
    public float RevolveRadious = 5f;
    public float RevolveSpeed = 30f;
    public AnimationCurve RevolveSpeedCurve;

    [Header("È¹µæ °ü·Ã")]
    public float Radious = 5f;
    public float Height = 1f;
    [Range(0f, 1f)]
    public float AbsolsionSpeed = 30f;
    public AnimationCurve RadiusExpandCurve;

}
