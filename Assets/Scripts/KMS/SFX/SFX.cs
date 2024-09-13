using UnityEngine;

[CreateAssetMenu(fileName = "SFXData", menuName = "SFX", order = 0)]
public class SFX : ScriptableObject
{
    [Header("Volume")]
    public float SFXvolum = 1f;
    public float BGMvolum = 1f;
    [Header("BGM")]
    public Clips lobby;
    public Clips gameRoom;
    public Clips bossRoom1;
    public Clips bossRoom2;

    [Header("Player")]
    public Clips playerstep;
    public Clips playerHit;
    public Clips playerDead;
    public Clips playerSetBlueChip;
    public Clips Absorbeing;
    public Clips playerMeleeTransform;
    public Clips playerRangeTransform;
    public Clips playerMeleeTransformRelease;
    public Clips playerRangeAttack;
    public Clips playerRangeAttack4;
    public Clips playerRangeProjectileHIt;
    public Clips playerMeleeAttack;
    public Clips playerMeleeAttackHit;
    public Clips playerCharging;
    public Clips playerCharged;
    public Clips playerChargedAttack;
    public Clips playerDash;
    public Clips RangeSkill1;
    public Clips RangeSkill2;
    public Clips RangeSkill3;
    public Clips RangeSkill4;
    public Clips MeleeSkill1;
    public Clips MeleeSkill2_1;
    public Clips MeleeSkill2_2;
    public Clips MeleeSkill3;
    public Clips MeleeSkill4_1;
    public Clips MeleeSkill4_2;

    [Header("NPC")]
    public Clips NPCStep;
    public Clips NPCHit;
    public Clips NPCAttack;
    public Clips NPCDeath;
    public Clips NPCAttackHit;

    [Header("Boss")]
    public Clips boss_Roar1;
    public Clips boss_Roar2;
    public Clips boss_Step;
    public Clips boss_run;
    public Clips boss_Back_Jump;
    public Clips boss_Landing;
    public Clips boss_SpikeSummoning;
    public Clips boss_Spikehit;
    public Clips boss_SpikeBroken;
    public Clips boss_Charge;
    public Clips boss_spike_throw;
    public Clips boss_MeleeAttack;
    public Clips boss_MeleeAttackEnd;
    public Clips boss_Death;
    public Clips boss_Ulti;




    [Header("System")]
    public Clips boxOpen;
    public Clips StageClear;
    public Clips getBullet;


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
