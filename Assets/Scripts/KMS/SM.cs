using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SM : SceneSingleton<SM>
{
    private AudioSource _audioSource;

    private GameObject _soundPrefeb;
    public SFX Absorbeing;


    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip _soundPrefeb,Vector3 _pos)
    {
        
    }

    public void PlaySound2(SFX sfx,Vector3 _pos)
    {
        AudioClip audioclips = sfx.clip;
        GameObject SB = ObjectPoolManager.Instance.DequeueObject(audioclips, _pos);
        SB.transform.position = _pos;
        SB.GetComponent<AudioSource>().Play();

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
