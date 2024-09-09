using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
    public AudioClip playerSetBlueChip;
    public AudioClip Absorbeing;
    public AudioClip playerMeleeTransform;
    public AudioClip playerRangeTransform;
    public AudioClip playerMeleeTransformRelease;
    public AudioClip playerRangeAttack;
    public AudioClip playerRangeAttack4;
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

    [Header("Boss")]
    public AudioClip boss_Roar1;
    public AudioClip boss_Roar2;
    public AudioClip boss_Step;
    public AudioClip boss_Back_Jump;
    public AudioClip boss_Landing;
    public AudioClip boss_SpikeSummoning;
    public AudioClip boss_Spikehit;
    public AudioClip boss_SpikeBroken;
    public AudioClip boss_Charge;
    public AudioClip boss_spike_throw;
    public AudioClip boss_MeleeAttack;
    public AudioClip boss_Death;



    [Header("System")]
    public AudioClip boxOpen;
    public AudioClip StageClear;
    public AudioClip getBullet;


    //public Dictionary<string, AudioClip> audioClipDictionary;

    //private void OnEnable()
    //{
    //    // Initialize and populate the dictionary
    //    audioClipDictionary = new Dictionary<string, AudioClip>
    //    {
    //        { "playerstep", playerstep },
    //        { "playerHit", playerHit },
    //        { "playerDead", playerDead },
    //        { "playerMeleeTransform", playerMeleeTransform },
    //        { "playerRangeTransform", playerRangeTransform },
    //        { "playerRangeProjectileHIt", playerRangeProjectileHIt },
    //        { "playerMeleeAttackHit", playerMeleeAttackHit },
    //        { "playerCharging", playerCharging },
    //        { "playerCharged", playerCharged },
    //        { "playerChargedAttack", playerChargedAttack },
    //        { "playerDash", playerDash },
    //        { "RangeSkill1", RangeSkill1 },
    //        { "RangeSkill2", RangeSkill2 },
    //        { "RangeSkill3", RangeSkill3 },
    //        { "RangeSkill4", RangeSkill4 },
    //        { "MeleeSkill1", MeleeSkill1 },
    //        { "MeleeSkill2_1", MeleeSkill2_1 },
    //        { "MeleeSkill2_2", MeleeSkill2_2 },
    //        { "MeleeSkill3", MeleeSkill3 },
    //        { "MeleeSkill4_1", MeleeSkill4_1 },
    //        { "MeleeSkill4_2", MeleeSkill4_2 },
    //        { "NPCStep", NPCStep },
    //        { "NPCHit", NPCHit },
    //        { "NPCAttack", NPCAttack },
    //        { "NPCDeath", NPCDeath },
    //        // Add more mappings as needed
    //    };
    //}
}
