using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioSource AudioSource;

    private void Awake()
    {
        TryGetComponent(out AudioSource);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
