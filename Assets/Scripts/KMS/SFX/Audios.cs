using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audios : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] public string SoundName;
    public GameObject SB;
    //public bool spatialBlend=false;

    public void OnEnable()
    {
        SB = SM.Instance.PlaySound2(SoundName,transform.position);
        //if (spatialBlend)
        //{
        //    AudioSource Audio = SB.GetComponent<AudioSource>();
        //    Audio.spatialBlend = 1f;
        //}
    }
    public void OnDisable()
    {
        if(SB != null)
        ObjectPoolManager.Instance.EnqueueObject(SB);
    }


    //private void Awake()
    //{
    //    _audioSource = GetComponent<AudioSource>();
    //}
    //private void OnEnable()
    //{
    //    PlayAudioClip(_audioSource);
    //}
    //public void PlayAudioClip(AudioSource _audioSource)
    //{

    //    _audioSource.Play();

    //    if (!_audioSource.loop)
    //    {
    //        StartCoroutine(ReturnToPoolAfterPlayback());
    //    }
    //}

    //private IEnumerator ReturnToPoolAfterPlayback()
    //{
    //    // Wait for the length of the audio clip to finish
    //    yield return new WaitForSeconds(_audioSource.clip.length);

    //    // Return the object to the pool
    //    ObjectPoolManager.Instance.EnqueueObject(gameObject);
    //}
}
