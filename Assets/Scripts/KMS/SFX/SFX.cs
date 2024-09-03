
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SFXData", menuName = "SFX", order = 0)]

public class SFX : ScriptableObject
{
    [Header("Volume")]
    public float VFXvolum = 1f;
    public float BGMvolum = 1f;
    [Header("BGM")]
    public AudioClip lobby;
    public AudioClip gameRoom;
    public AudioClip bossRoom;

    [Header("Player")]
    public AudioClip playerstep;
    public AudioClip playerHit;
    public AudioClip playerDead;
    public AudioClip playerMeleeTransform;
    public AudioClip playerRangeTransform;
    public AudioClip playerRangeProjectileHIt;
    public AudioClip playerMeleeAttackHit;
    public AudioClip playerCharging;
    public AudioClip playerCharged;
    public AudioClip playerChargedAttack;
    public AudioClip playerDash;
    public AudioClip RangeSkill1;
    public AudioClip RangeSkill2;
    public AudioClip RangeSkill3;
    public AudioClip RangeSkill4;
    public AudioClip MeleeSkill1;
    public AudioClip MeleeSkill2_1;
    public AudioClip MeleeSkill2_2;
    public AudioClip MeleeSkill3;
    public AudioClip MeleeSkill4_1;
    public AudioClip MeleeSkill4_2;

    [Header("NPC")]
    public AudioClip NPCStep;
    public AudioClip NPCHit;
    public AudioClip NPCAttack;
    public AudioClip NPCDeath;
}

