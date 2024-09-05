using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SM : GlobalSingleton<SM>
{

    public SFX sfxData; // Assign in the inspector

    private Dictionary<string, AudioClip> audioClipDictionary;

    private void Awake()
    {
        InitializeAudioDictionary();
    }

    private void InitializeAudioDictionary()//AWAKE할때 sfxData로부터 오디오클립을 받아옴
    {
        audioClipDictionary = new Dictionary<string, AudioClip>
        {
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
            { "NPCStep", sfxData.NPCStep },
            { "NPCHit", sfxData.NPCHit },
            { "NPCAttack", sfxData.NPCAttack },
            { "NPCDeath", sfxData.NPCDeath }
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
}