using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXClip", menuName = "SFXClip", order = 1)]
public class Clips : ScriptableObject
{
    public AudioClip clip;
    [Range(0,1)]public float SFXVolum = 1f;
    [Range(0, 1)] public float spatialBlend = 1f;
}
