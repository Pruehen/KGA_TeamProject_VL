using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
public class SM : GlobalSingleton<SM>
{

    public SFX sfxData; // Assign in the inspector

    private Dictionary<string, AudioClip> audioClipDictionary;
    public AudioSource BGM;
    public int SCType = 999;

    private void Awake()
    {
        InitializeAudioDictionary();
        if (BGM == null)
        {
            BGM = gameObject.AddComponent<AudioSource>(); 
        }
    }

    private void InitializeAudioDictionary() // AWAKE할 때 sfxData로부터 오디오클립을 받아옴
    {
        audioClipDictionary = new Dictionary<string, AudioClip>
    {
        // Player Sounds
        { "playerstep", sfxData.playerstep },
        { "playerHit", sfxData.playerHit },
        { "playerDead", sfxData.playerDead },
        { "playerSetBlueChip", sfxData.playerSetBlueChip },
        { "Absorbeing", sfxData.Absorbeing },
        { "playerMeleeTransform", sfxData.playerMeleeTransform },
        { "playerMeleeTransformRelease", sfxData.playerMeleeTransformRelease },
        { "playerRangeTransform", sfxData.playerRangeTransform },
        { "playerRangeProjectileHIt", sfxData.playerRangeProjectileHIt },
        { "playerRangeAttack", sfxData.playerRangeAttack },
        { "playerRangeAttack4", sfxData.playerRangeAttack4 },
        { "playerMeleeAttackHit", sfxData.playerMeleeAttackHit },
        { "playerCharging", sfxData.playerCharging },
        { "playerCharged", sfxData.playerCharged },
        { "playerChargedAttack", sfxData.playerChargedAttack },
        { "playerDash", sfxData.playerDash },
        { "RangeSkill1", sfxData.RangeSkill1 },
        { "RangeSkill2", sfxData.RangeSkill2 },
        { "RangeSkill3", sfxData.RangeSkill3 },
        { "RangeSkill4", sfxData.RangeSkill4 },
        { "MeleeSkill1", sfxData.MeleeSkill1 },
        { "MeleeSkill2_1", sfxData.MeleeSkill2_1 },
        { "MeleeSkill2_2", sfxData.MeleeSkill2_2 },
        { "MeleeSkill3", sfxData.MeleeSkill3 },
        { "MeleeSkill4_1", sfxData.MeleeSkill4_1 },
        { "MeleeSkill4_2", sfxData.MeleeSkill4_2 },

        // NPC Sounds
        { "NPCStep", sfxData.NPCStep },
        { "NPCHit", sfxData.NPCHit },
        { "NPCAttack", sfxData.NPCAttack },
        { "NPCDeath", sfxData.NPCDeath },

        // BGM
        { "lobby", sfxData.lobby },
        { "gameRoom", sfxData.gameRoom },
        { "bossRoom", sfxData.bossRoom },

        // Boss Sounds
        { "boss_Roar1", sfxData.boss_Roar1 },
        { "boss_Roar2", sfxData.boss_Roar2 },
        { "boss_Step", sfxData.boss_Step },
        { "boss_Back_Jump", sfxData.boss_Back_Jump },
        { "boss_Landing", sfxData.boss_Landing },
        { "boss_SpikeSummoning", sfxData.boss_SpikeSummoning },
        { "boss_Spikehit", sfxData.boss_Spikehit },
        { "boss_SpikeBroken", sfxData.boss_SpikeBroken },
        { "boss_Charge", sfxData.boss_Charge },
        { "boss_spike_throw", sfxData.boss_spike_throw },
        { "boss_MeleeAttack", sfxData.boss_MeleeAttack },
        { "boss_Death", sfxData.boss_Death },

        // System Sounds
        { "boxOpen", sfxData.boxOpen },
        { "StageClear", sfxData.StageClear },
        { "getBullet", sfxData.getBullet }
    };
    }


    public GameObject PlaySound2(string soundName, Vector3 position)
    {

        if (audioClipDictionary.TryGetValue(soundName, out AudioClip audioClip))
        {
            GameObject sb = ObjectPoolManager.Instance.DequeueObject(audioClip, position);
            sb.transform.position = position;

            AudioSource audioSource = sb.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = sb.AddComponent<AudioSource>();
                audioSource = sb.GetComponent<AudioSource>();
            }
            audioSource.clip = audioClip;
            audioSource.Play();
            if (!audioSource.loop)//루프가 켜져있지 않을시 Lenth만큼 재생후 풀에반환함
            {
                StartCoroutine(ReturnToPoolAfterPlayback(audioSource));
            }
            return sb;
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in SFX data.");
            return null;
        }

    }
    private IEnumerator ReturnToPoolAfterPlayback(AudioSource _audioSource)
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        ObjectPoolManager.Instance.EnqueueObject(_audioSource.transform.gameObject);
    }
    public void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlaySoundAttachedToObject(AudioClip clip, GameObject target)
    {
        // target에 AudioSource가 없다면 추가하고, 해당 AudioSource로 재생.
        AudioSource source = target.GetComponent<AudioSource>();
        if (source == null)
        {
            source = target.AddComponent<AudioSource>();
        }
        source.clip = clip;
        source.Play();
    }
    public void SetBGM(int Name)
    {
        Debug.Log("SetBGM"+Name);
        if(SCType!= Name)
        switch (Name)
        {
            case 0:
                if (audioClipDictionary.TryGetValue("lobby", out AudioClip lobbyaudioClip))
                {
                    BGM.clip = lobbyaudioClip;
                    BGM.Play();
                        SCType=Name;
                    return;
                }
                else
                    return;
            case 1:
                if (audioClipDictionary.TryGetValue("gameRoom", out AudioClip normalaudioClip))
                {
                    BGM.clip = normalaudioClip;
                    BGM.Play();
                        SCType = Name;
                        return;
                }
                else

                    return;
            case 2:
                if (audioClipDictionary.TryGetValue("bossRoom", out AudioClip bossaudioClip))
                {
                    BGM.clip = bossaudioClip;
                    BGM.Play();
                        SCType = Name;
                        return;
                }
                else
                    return;
        }
    }
}