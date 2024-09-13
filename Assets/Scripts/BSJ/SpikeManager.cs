using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeManager : SceneSingleton<SpikeManager>
{
    [SerializeField] private GameObject _poolPrefab;
    [SerializeField] private int _count = 5000;
    public List<SpikeSpawner> Spikes {get; set;}

    private void Awake()
    {
        ObjectPoolManager.Instance.CreatePool(_poolPrefab, _count);
        Spikes = new List<SpikeSpawner>(5000);
    }
    public void DestroyAllSpike()
    {
        foreach (SpikeSpawner s in Spikes)
        {
            s.DestroySpike();
        }
        Spikes.Clear();
    }

}
