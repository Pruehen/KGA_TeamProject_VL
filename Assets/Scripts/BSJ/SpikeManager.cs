using System.Collections.Generic;
using UnityEngine;

public class SpikeManager : SceneSingleton<SpikeManager>
{
    [SerializeField] private GameObject _poolPrefab;
    [SerializeField] private GameObject[] _trashPrefab;
    [SerializeField] private int _spikeCount = 2000;
    [SerializeField] private int _trashCount = 1000;
    public List<SpikeSpawner> Spikes { get; set; }
    public List<TrashItem> Trashs { get; set; }

    private void Awake()
    {
        ObjectPoolManager.Instance.CreatePool(_poolPrefab, _spikeCount);
        foreach (var trash in _trashPrefab)
        {
            ObjectPoolManager.Instance.CreatePool(_poolPrefab, _trashCount);
        }

        Spikes = new List<SpikeSpawner>(_spikeCount);
        Trashs = new List<TrashItem>(_trashCount * _trashPrefab.Length);
    }
    public void DestroyAllSpike()
    {
        foreach (SpikeSpawner spike in Spikes)
        {
            spike.DestroySpike();
        }
        Spikes.Clear();
    }
    public void DestroyAllTrash()
    {
        foreach (TrashItem trash in Trashs)
        {
            trash.Enqueue();
        }
        Trashs.Clear();
    }

}
