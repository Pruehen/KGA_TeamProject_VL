using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class SM : GlobalSingleton<SM>
{

    public SFX sfxData; // Assign in the inspector

    private Dictionary<string, Clips> audioClipDictionary;
    public AudioSource BGM;
    public int SCType = 999;

    private Coroutine _loopControl;

    private void Awake()
    {
        InitializeAudioDictionary();
        if (BGM == null)
        {
            BGM = gameObject.AddComponent<AudioSource>();
        }
        CreatPool();
    }

    private void InitializeAudioDictionary() // AWAKE�� �� sfxData�κ��� �����Ŭ���� �޾ƿ�
    {
        audioClipDictionary = new Dictionary<string, Clips>
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
        { "playerMeleeAttack", sfxData.playerMeleeAttack },
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
        { "NPCAttackHit", sfxData.NPCAttackHit },
        { "NPCDeath", sfxData.NPCDeath },

        // BGM
        { "lobby", sfxData.lobby },
        { "gameRoom", sfxData.gameRoom },
        { "bossRoom1", sfxData.bossRoom1 },
        { "bossRoom2", sfxData.bossRoom2 },

        // Boss Sounds
        { "boss_Roar1", sfxData.boss_Roar1 },
        { "boss_Roar2", sfxData.boss_Roar2 },
        { "boss_Step", sfxData.boss_Step },
        { "boss_run", sfxData.boss_run },
        { "boss_Back_Jump", sfxData.boss_Back_Jump },
        { "boss_Landing", sfxData.boss_Landing },
        { "boss_SpikeSummoning", sfxData.boss_SpikeSummoning },
        { "boss_Spikehit", sfxData.boss_Spikehit },
        { "boss_SpikeBroken", sfxData.boss_SpikeBroken },
        { "boss_Charge", sfxData.boss_Charge },
        { "boss_spike_throw", sfxData.boss_spike_throw },
        { "boss_MeleeAttack", sfxData.boss_MeleeAttack },
        { "boss_MeleeAttackEnd", sfxData.boss_MeleeAttackEnd },
        { "boss_Ulti", sfxData.boss_Ulti },
        { "boss_Death", sfxData.boss_Death },

        // System Sounds
        { "boxOpen", sfxData.boxOpen },
        { "StageClear", sfxData.StageClear },
        { "getBullet", sfxData.getBullet }
    };
    }
    public GameObject PlaySound2(string soundName, Vector3 position)
    {


        if (audioClipDictionary.TryGetValue(soundName, out Clips audioClip))
        {
            if (audioClip != null)
            {
                GameObject sb = ObjectPoolManager.Instance.DequeueObject(audioClip.clip, position);
                sb.transform.position = position;

                AudioSystem audioSystem = sb.GetComponent<AudioSystem>();
                AudioSource audioSource = audioSystem.AudioSource;
                audioSource.clip = audioClip.clip;
                audioSource.volume = audioClip.SFXVolum;
                audioSource.spatialBlend = audioClip.spatialBlend;
                audioSource.Play();
                if (!audioSource.loop)//������ �������� ������ Length��ŭ ����� Ǯ����ȯ��
                {
                    audioSystem.StartCoroutine(ReturnToPoolAfterPlayback(audioSource));
                }
                return sb;
            }
            else
            {
                return null;
            }
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
        if (_audioSource != null && ObjectPoolManager.Instance != null)
        {
                ObjectPoolManager.Instance.EnqueueObject(_audioSource.transform.gameObject);
        }

    }
    public void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void PlaySoundAttachedToObject(AudioClip clip, GameObject target)
    {
        // target�� AudioSource�� ���ٸ� �߰��ϰ�, �ش� AudioSource�� ���.
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
        Debug.Log("SetBGM" + Name);
        if (SCType != Name)
            switch (Name)
            {
                case 0:
                    if (audioClipDictionary.TryGetValue("lobby", out Clips lobbyaudioClip))
                    {
                        BGM.clip = lobbyaudioClip.clip;
                        BGM.volume = lobbyaudioClip.SFXVolum;
                        BGM.Play();
                        SCType = Name;
                        return;
                    }
                    else
                        return;
                case 1:
                    if (audioClipDictionary.TryGetValue("gameRoom", out Clips normalaudioClip))
                    {
                        BGM.clip = normalaudioClip.clip;
                        BGM.volume = normalaudioClip.SFXVolum;
                        BGM.Play();
                        SCType = Name;
                        return;
                    }
                    else

                        return;
                case 2:
                    if (audioClipDictionary.TryGetValue("bossRoom1", out Clips bossaudioClip))
                    {
                        BGM.clip = bossaudioClip.clip;
                        BGM.volume = bossaudioClip.SFXVolum;
                        BGM.Play();
                        SCType = Name;
                        return;
                    }
                    else
                        return;
            }
    }
    public void CreatPool()
    {
        foreach (KeyValuePair<string, Clips> entry in audioClipDictionary)
        {
            Clips audioClip = entry.Value;
            if (audioClip != null && audioClip.clip != null)
            {
                ObjectPoolManager.Instance.CreatePool(audioClip.clip,5);
            }
        }
    }
}