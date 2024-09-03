
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SFXData", menuName = "SFX", order = 0)]

public class SFX : ScriptableObject
{
    public AudioClip clip;
    public float volum = 1f;
}

